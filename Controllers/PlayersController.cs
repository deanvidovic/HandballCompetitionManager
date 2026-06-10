using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class PlayersController : Controller
    {
        private readonly PlayerRepository _playerRepository;
        private readonly TeamRepository _teamRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(PlayerRepository playerRepository, TeamRepository teamRepository, UserManager<AppUser> userManager, ILogger<PlayersController> logger)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(string? query)
        {
            _logger.LogInformation("Players Index page requested");
            ViewData["Query"] = query;
            var players = GetAccessiblePlayers(query);
            return View(players);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Autocomplete(string? query)
        {
            var players = GetAccessiblePlayers(query);

            var suggestions = players
                .Take(8)
                .Select(p => new
                {
                    label = p.FullName,
                    value = p.FullName,
                    meta = $"{p.Team?.Name ?? "No team"} - {p.Position}"
                });

            return Json(suggestions);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Coach")]
        public IActionResult TeamAutocomplete(string? query)
        {
            var teams = IsAdmin()
                ? _teamRepository.Search(query)
                : _teamRepository.SearchForCoach(query, CurrentUserDisplayName);

            var suggestions = teams
                .OrderBy(t => t.Name)
                .Take(8)
                .Select(t => new
                {
                    label = t.Name,
                    value = t.Name,
                    id = t.Id,
                    meta = $"{t.HomeCity} - {t.CoachName}"
                });

            return Json(suggestions);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Coach")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePlayerViewModel model)
        {
            var jerseyNumber = 0;
            var goalsScored = 0;
            var assists = 0;

            if (ModelState.IsValid)
            {
                if (!int.TryParse(model.JerseyNumber.Trim(), out jerseyNumber) || jerseyNumber < 1 || jerseyNumber > 99)
                {
                    ModelState.AddModelError(nameof(model.JerseyNumber), "Jersey number must be between 1 and 99.");
                }

                if (!int.TryParse(model.GoalsScored.Trim(), out goalsScored) || goalsScored < 0)
                {
                    ModelState.AddModelError(nameof(model.GoalsScored), "Goals scored cannot be negative.");
                }

                if (!int.TryParse(model.Assists.Trim(), out assists) || assists < 0)
                {
                    ModelState.AddModelError(nameof(model.Assists), "Assists cannot be negative.");
                }

                if (model.BirthDate.HasValue && model.BirthDate.Value.Date > DateTime.Today)
                {
                    ModelState.AddModelError(nameof(model.BirthDate), "Birth date cannot be after today's date.");
                }

                if (model.TeamId.HasValue && GetAccessibleTeam(model.TeamId.Value) == null)
                {
                    ModelState.AddModelError(nameof(model.TeamId), "Select an existing team from the suggestions.");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CreateValidationResponse());
            }

            var player = new Player
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                BirthDate = model.BirthDate!.Value.Date,
                JerseyNumber = jerseyNumber,
                Position = model.Position!.Value,
                TeamId = model.TeamId!.Value,
                GoalsScored = goalsScored,
                Assists = assists
            };

            _playerRepository.Add(player);
            _logger.LogInformation("Player created: {PlayerName}", player.FullName);

            return Json(new
            {
                success = true,
                message = "Player created successfully."
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Coach")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreatePlayerViewModel model)
        {
            var player = GetAccessiblePlayer(model.Id);
            var jerseyNumber = 0;
            var goalsScored = 0;
            var assists = 0;

            if (player == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!int.TryParse(model.JerseyNumber.Trim(), out jerseyNumber) || jerseyNumber < 1 || jerseyNumber > 99)
                {
                    ModelState.AddModelError(nameof(model.JerseyNumber), "Jersey number must be between 1 and 99.");
                }

                if (!int.TryParse(model.GoalsScored.Trim(), out goalsScored) || goalsScored < 0)
                {
                    ModelState.AddModelError(nameof(model.GoalsScored), "Goals scored cannot be negative.");
                }

                if (!int.TryParse(model.Assists.Trim(), out assists) || assists < 0)
                {
                    ModelState.AddModelError(nameof(model.Assists), "Assists cannot be negative.");
                }

                if (model.BirthDate.HasValue && model.BirthDate.Value.Date > DateTime.Today)
                {
                    ModelState.AddModelError(nameof(model.BirthDate), "Birth date cannot be after today's date.");
                }

                if (model.TeamId.HasValue && GetAccessibleTeam(model.TeamId.Value) == null)
                {
                    ModelState.AddModelError(nameof(model.TeamId), "Select an existing team from the suggestions.");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CreateValidationResponse());
            }

            player.FirstName = model.FirstName.Trim();
            player.LastName = model.LastName.Trim();
            player.BirthDate = model.BirthDate!.Value.Date;
            player.JerseyNumber = jerseyNumber;
            player.Position = model.Position!.Value;
            player.TeamId = model.TeamId!.Value;
            player.GoalsScored = goalsScored;
            player.Assists = assists;

            _playerRepository.Update(player);
            _logger.LogInformation("Player updated: {PlayerName}", player.FullName);

            return Json(new
            {
                success = true,
                message = "Player updated successfully."
            });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Players Details page requested for player ID: {PlayerId}", id);
            var player = GetAccessiblePlayer(id);
            
            if (player == null)
            {
                _logger.LogWarning("Player with ID {PlayerId} not found", id);
                return NotFound();
            }

            return View(player);
        }

        [HttpGet]
        [Authorize]
        [Route("teams/{teamId}/players")]
        public IActionResult GetByTeam(int teamId)
        {
            _logger.LogInformation("Players by team requested: {TeamId}", teamId);
            var players = IsAdmin()
                ? _playerRepository.GetByTeamId(teamId)
                : IsCoach()
                    ? _playerRepository.GetByTeamIdForCoach(teamId, CurrentUserDisplayName)
                    : _playerRepository.GetByTeamIdForUser(teamId, CurrentUserId);
            return View("Index", players);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Coach")]
        public IActionResult Edit(int id)
        {
            var player = GetAccessiblePlayer(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var player = _playerRepository.GetById(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var player = _playerRepository.GetById(id);

            if (player == null)
            {
                TempData["ErrorNotification"] = "Player was not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var playerName = player.FullName;
                _playerRepository.Delete(id);
                _logger.LogInformation("Player deleted: {PlayerName}", playerName);
                TempData["SuccessNotification"] = $"{playerName} deleted successfully.";
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Player with ID {PlayerId} could not be deleted", id);
                TempData["ErrorNotification"] = "Player could not be deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        private object CreateValidationResponse()
        {
            var fieldErrors = ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

            return new
            {
                success = false,
                errors = fieldErrors.SelectMany(entry => entry.Value).ToList(),
                fieldErrors
            };
        }

        private bool IsAdmin() => User.IsInRole("Admin");

        private bool IsCoach() => User.IsInRole("Coach");

        private List<Player> GetAccessiblePlayers(string? query)
        {
            if (IsAdmin())
            {
                return _playerRepository.Search(query);
            }

            if (IsCoach())
            {
                return _playerRepository.SearchForCoach(query, CurrentUserDisplayName);
            }

            return _playerRepository.SearchForUser(query, CurrentUserId);
        }

        private Player? GetAccessiblePlayer(int id)
        {
            if (IsAdmin())
            {
                return _playerRepository.GetById(id);
            }

            if (IsCoach())
            {
                return _playerRepository.GetByIdForCoach(id, CurrentUserDisplayName);
            }

            return _playerRepository.GetByIdForUser(id, CurrentUserId);
        }

        private Team? GetAccessibleTeam(int id)
        {
            if (IsAdmin())
            {
                return _teamRepository.GetById(id);
            }

            if (IsCoach())
            {
                return _teamRepository.GetByIdForCoach(id, CurrentUserDisplayName);
            }

            return _teamRepository.GetByIdForUser(id, CurrentUserId);
        }

        private int CurrentUserId
        {
            get
            {
                var userId = _userManager.GetUserId(User);
                return int.TryParse(userId, out var parsedUserId) ? parsedUserId : 0;
            }
        }

        private string CurrentUserDisplayName =>
            _userManager.GetUserAsync(User).GetAwaiter().GetResult()?.DisplayName ?? string.Empty;
    }
}

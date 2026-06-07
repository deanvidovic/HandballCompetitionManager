using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class PlayersController : Controller
    {
        private readonly PlayerRepository _playerRepository;
        private readonly TeamRepository _teamRepository;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(PlayerRepository playerRepository, TeamRepository teamRepository, ILogger<PlayersController> logger)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string? query)
        {
            _logger.LogInformation("Players Index page requested");
            ViewData["Query"] = query;
            var players = _playerRepository.Search(query);
            return View(players);
        }

        [HttpGet]
        public IActionResult Autocomplete(string? query)
        {
            var suggestions = _playerRepository.Search(query)
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
        public IActionResult TeamAutocomplete(string? query)
        {
            var suggestions = _teamRepository.Search(query)
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

                if (model.TeamId.HasValue && _teamRepository.GetById(model.TeamId.Value) == null)
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreatePlayerViewModel model)
        {
            var player = _playerRepository.GetById(model.Id);
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

                if (model.TeamId.HasValue && _teamRepository.GetById(model.TeamId.Value) == null)
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
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Players Details page requested for player ID: {PlayerId}", id);
            var player = _playerRepository.GetById(id);
            
            if (player == null)
            {
                _logger.LogWarning("Player with ID {PlayerId} not found", id);
                return NotFound();
            }

            return View(player);
        }

        [HttpGet]
        [Route("teams/{teamId}/players")]
        public IActionResult GetByTeam(int teamId)
        {
            _logger.LogInformation("Players by team requested: {TeamId}", teamId);
            var players = _playerRepository.GetByTeamId(teamId);
            return View("Index", players);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Return a view to edit an existing player
            var player = _playerRepository.GetById(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Return a view to confirm deletion of a player
            var player = _playerRepository.GetById(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        [HttpPost]
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
    }
}

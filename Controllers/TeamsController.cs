using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class TeamsController : Controller
    {
        private readonly TeamRepository _teamRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly AppUserRepository _appUserRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(TeamRepository teamRepository, PlayerRepository playerRepository, AppUserRepository appUserRepository, UserManager<AppUser> userManager, ILogger<TeamsController> logger)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _appUserRepository = appUserRepository;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(string? query)
        {
            _logger.LogInformation("Teams Index page requested");
            ViewData["Query"] = query;
            var teams = IsAdmin()
                ? _teamRepository.Search(query)
                : IsCoach()
                    ? _teamRepository.SearchForCoach(query, CurrentUserDisplayName)
                    : _teamRepository.SearchForUser(query, CurrentUserId);
            return View(teams);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Autocomplete(string? query)
        {
            var teams = IsAdmin()
                ? _teamRepository.Search(query)
                : IsCoach()
                    ? _teamRepository.SearchForCoach(query, CurrentUserDisplayName)
                    : _teamRepository.SearchForUser(query, CurrentUserId);

            var suggestions = teams
                .Take(8)
                .Select(t => new
                {
                    label = t.Name,
                    value = t.Name,
                    meta = $"{t.CoachName} - {t.HomeCity}"
                });

            return Json(suggestions);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CoachAutocomplete(string? query)
        {
            var coaches = _appUserRepository.GetByRole(UserRole.Coach);

            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                coaches = coaches
                    .Where(u =>
                        u.DisplayName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        (u.UserName?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        u.Email.Contains(term, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var suggestions = coaches
                .OrderBy(u => u.DisplayName)
                .Take(8)
                .Select(u => new
                {
                    label = u.DisplayName,
                    value = u.DisplayName,
                    meta = $"{u.UserName} - {u.Email}"
                });

            return Json(suggestions);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateTeamViewModel model)
        {
            var foundedYear = 0;

            if (ModelState.IsValid)
            {
                if (!int.TryParse(model.FoundedYear.Trim(), out foundedYear))
                {
                    ModelState.AddModelError(nameof(model.FoundedYear), "Founded year can contain only numbers.");
                }
                else if (foundedYear > 2026)
                {
                    ModelState.AddModelError(nameof(model.FoundedYear), "Founded year cannot be greater than 2026.");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CreateValidationResponse());
            }

            if (IsCoach() && !model.CoachName.Trim().Equals(CurrentUserDisplayName, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.CoachName), "Coach cannot be changed.");
                return BadRequest(CreateValidationResponse());
            }

            var selectedCoach = _appUserRepository.GetByRole(UserRole.Coach)
                .FirstOrDefault(u => u.DisplayName.Equals(model.CoachName.Trim(), StringComparison.OrdinalIgnoreCase));

            if (selectedCoach == null)
            {
                ModelState.AddModelError(nameof(model.CoachName), "Select an existing coach from the suggestions.");

                return BadRequest(new
                {
                    success = false,
                    errors = new[] { "Select an existing coach from the suggestions." },
                    fieldErrors = new Dictionary<string, string[]>
                    {
                        [nameof(model.CoachName)] = new[] { "Select an existing coach from the suggestions." }
                    }
                });
            }

            var team = new Team
            {
                Name = model.Name.Trim(),
                CoachName = selectedCoach.DisplayName,
                HomeCity = model.HomeCity.Trim(),
                FoundedYear = foundedYear,
                HomeArena = model.HomeArena.Trim()
            };

            _teamRepository.Add(team);
            _logger.LogInformation("Team created: {TeamName}", team.Name);

            return Json(new
            {
                success = true,
                message = "Team created successfully."
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Coach")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateTeamViewModel model)
        {
            var team = GetAccessibleTeam(model.Id);

            if (team == null)
            {
                return NotFound();
            }

            var foundedYear = 0;

            if (ModelState.IsValid)
            {
                if (!int.TryParse(model.FoundedYear.Trim(), out foundedYear))
                {
                    ModelState.AddModelError(nameof(model.FoundedYear), "Founded year can contain only numbers.");
                }
                else if (foundedYear > 2026)
                {
                    ModelState.AddModelError(nameof(model.FoundedYear), "Founded year cannot be greater than 2026.");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CreateValidationResponse());
            }

            var selectedCoach = _appUserRepository.GetByRole(UserRole.Coach)
                .FirstOrDefault(u => u.DisplayName.Equals(model.CoachName.Trim(), StringComparison.OrdinalIgnoreCase));

            if (selectedCoach == null)
            {
                ModelState.AddModelError(nameof(model.CoachName), "Select an existing coach from the suggestions.");
                return BadRequest(CreateValidationResponse());
            }

            if (IsCoach() && !selectedCoach.DisplayName.Equals(CurrentUserDisplayName, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(model.CoachName), "Coach cannot be changed.");
                return BadRequest(CreateValidationResponse());
            }

            team.Name = model.Name.Trim();
            team.CoachName = selectedCoach.DisplayName;
            team.HomeCity = model.HomeCity.Trim();
            team.FoundedYear = foundedYear;
            team.HomeArena = model.HomeArena.Trim();

            _teamRepository.Update(team);
            _logger.LogInformation("Team updated: {TeamName}", team.Name);

            return Json(new
            {
                success = true,
                message = "Team updated successfully."
            });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Teams Details page requested for team ID: {TeamId}", id);
            var team = GetAccessibleTeam(id);
            
            if (team == null)
            {
                _logger.LogWarning("Team with ID {TeamId} not found", id);
                return NotFound();
            }

            return View(team);
        }

        [HttpGet]
        [Authorize]
        [Route("teams/city/{city}")]
        public IActionResult GetByCity(string city)
        {
            _logger.LogInformation("Teams by city requested: {City}", city);
            var teams = IsAdmin()
                ? _teamRepository.GetByCity(city)
                : IsCoach()
                    ? _teamRepository.GetByCityForCoach(city, CurrentUserDisplayName)
                    : _teamRepository.GetByCityForUser(city, CurrentUserId);
            return View("Index", teams);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var team = _teamRepository.GetById(id);

            if (team == null)
            {
                TempData["ErrorNotification"] = "Team was not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var teamName = team.Name;
                _teamRepository.Delete(id);
                _logger.LogInformation("Team deleted: {TeamName}", teamName);
                TempData["SuccessNotification"] = $"{teamName} deleted successfully.";
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Team with ID {TeamId} could not be deleted", id);
                TempData["ErrorNotification"] = "Team could not be deleted.";
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

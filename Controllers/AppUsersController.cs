using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly AppUserRepository _appUserRepository;
        private readonly ILogger<AppUsersController> _logger;

        public AppUsersController(AppUserRepository appUserRepository, ILogger<AppUsersController> logger)
        {
            _appUserRepository = appUserRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string? query)
        {
            _logger.LogInformation("AppUsers Index page requested");
            ViewData["Query"] = query;
            var users = _appUserRepository.Search(query);
            return View(users);
        }

        [HttpGet]
        public IActionResult Autocomplete(string? query)
        {
            var suggestions = _appUserRepository.Search(query)
                .Take(8)
                .Select(u => new
                {
                    label = u.DisplayName,
                    value = u.DisplayName,
                    meta = $"{u.Username} - {u.Role}"
                });

            return Json(suggestions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateAppUserViewModel model)
        {
            if (model.DateOfBirth.HasValue && model.DateOfBirth.Value.Date > DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), "Date of birth cannot be after today's date.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CreateValidationResponse());
            }

            var user = new AppUser
            {
                Username = model.Username.Trim(),
                DisplayName = model.DisplayName.Trim(),
                Email = model.Email.Trim(),
                Role = model.Role!.Value,
                DateOfBirth = model.DateOfBirth!.Value.Date,
                CreatedAt = DateTime.Now
            };

            _appUserRepository.Add(user);
            _logger.LogInformation("App user created: {Username}", user.Username);

            return Json(new
            {
                success = true,
                message = "App user created successfully."
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateAppUserViewModel model)
        {
            var user = _appUserRepository.GetById(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            if (model.DateOfBirth.HasValue && model.DateOfBirth.Value.Date > DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.DateOfBirth), "Date of birth cannot be after today's date.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CreateValidationResponse());
            }

            user.Username = model.Username.Trim();
            user.DisplayName = model.DisplayName.Trim();
            user.Email = model.Email.Trim();
            user.Role = model.Role!.Value;
            user.DateOfBirth = model.DateOfBirth!.Value.Date;

            _appUserRepository.Update(user);
            _logger.LogInformation("App user updated: {Username}", user.Username);

            return Json(new
            {
                success = true,
                message = "App user updated successfully."
            });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("AppUsers Details page requested for user ID: {UserId}", id);
            var user = _appUserRepository.GetById(id);
            
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var user = _appUserRepository.GetById(id);

            if (user == null)
            {
                TempData["ErrorNotification"] = "App user was not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var displayName = user.DisplayName;
                _appUserRepository.Delete(id);
                _logger.LogInformation("App user deleted: {DisplayName}", displayName);
                TempData["SuccessNotification"] = $"{displayName} deleted successfully.";
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "App user with ID {UserId} could not be deleted", id);
                TempData["ErrorNotification"] = "App user could not be deleted.";
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

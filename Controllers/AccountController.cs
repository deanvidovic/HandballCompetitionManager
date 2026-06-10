using HandballCompetitionManager.Models;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(
        ILogger<AccountController> logger,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        _logger.LogInformation("Login page requested");
        return View(new LoginViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email.Trim());

        if (user == null || user.DeletedAt != null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName ?? user.Email ?? model.Email,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            _logger.LogInformation("User signed in: {Email}", model.Email);
            TempData["SuccessNotification"] = "You have signed in successfully.";
            return RedirectToAction("Index", "Competitions");
        }

        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        _logger.LogInformation("Register page requested");
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedEmail = model.Email.Trim();
        var username = model.Username.Trim();
        var user = new AppUser
        {
            UserName = username,
            DisplayName = model.DisplayName.Trim(),
            Email = normalizedEmail,
            EmailConfirmed = true,
            Role = UserRole.TournamentManager,
            OIB = model.OIB.Trim(),
            JMBG = model.JMBG.Trim(),
            DateOfBirth = null,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Manager");
            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User registered: {Email}", user.Email);
            TempData["SuccessNotification"] = "Your account has been created.";
            return RedirectToAction("Index", "Competitions");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        TempData["SuccessNotification"] = "You have signed out.";
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePersonalData(PersonalDataViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null || user.DeletedAt != null)
        {
            return Unauthorized();
        }

        if (model.DateOfBirth.HasValue && model.DateOfBirth.Value.Date > DateTime.Today)
        {
            ModelState.AddModelError(nameof(model.DateOfBirth), "Date of birth cannot be after today's date.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(CreateValidationResponse());
        }

        var normalizedEmail = model.Email.Trim();
        user.DisplayName = model.DisplayName.Trim();
        user.Email = normalizedEmail;
        user.OIB = model.OIB.Trim();
        user.JMBG = model.JMBG.Trim();
        user.DateOfBirth = model.DateOfBirth?.Date;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(CreateValidationResponse());
        }

        await _signInManager.RefreshSignInAsync(user);
        TempData["SuccessNotification"] = "Personal data updated successfully.";

        return Json(new
        {
            success = true,
            message = "Personal data updated successfully."
        });
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

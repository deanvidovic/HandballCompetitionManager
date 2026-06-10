using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.ViewModels;

public class LoginViewModel
{
    public string Eyebrow { get; set; } = "Secure access";
    public string Title { get; set; } = "Sign in to your workspace";
    public string Description { get; set; } = "Continue managing competitions, match reports, teams, and tournament schedules from one focused dashboard.";
    public string SubmitText { get; set; } = "Sign in";
    public string SwitchText { get; set; } = "New to Handball Manager?";
    public string SwitchLinkText { get; set; } = "Create an account";
    public string BrandName { get; set; } = "Handball Manager";
    public string HomeAriaLabel { get; set; } = "Go to home page";
    public string BenefitAriaLabel { get; set; } = "Account benefit";
    public string BenefitText { get; set; } = "Keep competition setup, match reports, and team records in one organized workspace.";
    public string EmailPlaceholder { get; set; } = "admin@handball.local";
    public string PasswordPlaceholder { get; set; } = "Enter password";

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(160, ErrorMessage = "Email cannot be longer than 160 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(120, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

}

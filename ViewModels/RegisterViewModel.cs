using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.ViewModels;

public class RegisterViewModel
{
    public string Eyebrow { get; set; } = "Tournament profile";
    public string Title { get; set; } = "Create your account";
    public string Description { get; set; } = "Set up access for organizing teams, players, competitions, and match documentation.";
    public string SubmitText { get; set; } = "Create account";
    public string SwitchText { get; set; } = "Already have an account?";
    public string SwitchLinkText { get; set; } = "Sign in";
    public string BrandName { get; set; } = "Handball Manager";
    public string HomeAriaLabel { get; set; } = "Go to home page";
    public string BenefitAriaLabel { get; set; } = "Account benefit";
    public string BenefitText { get; set; } = "Prepare your profile before connecting real authentication and account permissions.";
    public string UsernamePlaceholder { get; set; } = "dean.vidovic";
    public string DisplayNamePlaceholder { get; set; } = "Dean Vidovic";
    public string EmailPlaceholder { get; set; } = "admin@example.com";
    public string OibPlaceholder { get; set; } = "12345678901";
    public string JmbgPlaceholder { get; set; } = "1234567890123";
    public string PasswordPlaceholder { get; set; } = "Create strong password";
    public string ConfirmPasswordPlaceholder { get; set; } = "Confirm password";

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Username must be at least 3 characters long.")]
    [RegularExpression(@"^[A-Za-z0-9._-]+$", ErrorMessage = "Username can contain only letters, numbers, dots, underscores, and dashes.")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Display name is required.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Display name must be at least 3 characters long.")]
    [Display(Name = "Display name")]
    public string DisplayName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(160, ErrorMessage = "Email cannot be longer than 160 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "OIB is required.")]
    [RegularExpression("^[0-9]{11}$", ErrorMessage = "OIB must contain exactly 11 numbers.")]
    [Display(Name = "OIB")]
    public string OIB { get; set; } = string.Empty;

    [Required(ErrorMessage = "JMBG is required.")]
    [RegularExpression("^[0-9]{13}$", ErrorMessage = "JMBG must contain exactly 13 numbers.")]
    [Display(Name = "JMBG")]
    public string JMBG { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(120, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$", ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

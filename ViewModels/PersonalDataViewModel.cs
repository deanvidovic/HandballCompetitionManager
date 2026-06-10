using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.ViewModels;

public class PersonalDataViewModel
{
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
    [StringLength(11, MinimumLength = 11, ErrorMessage = "OIB must contain exactly 11 digits.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "OIB can contain only numbers.")]
    [Display(Name = "OIB")]
    public string OIB { get; set; } = string.Empty;

    [Required(ErrorMessage = "JMBG is required.")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG must contain exactly 13 digits.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "JMBG can contain only numbers.")]
    [Display(Name = "JMBG")]
    public string JMBG { get; set; } = string.Empty;

    [Display(Name = "Date of birth")]
    public DateTime? DateOfBirth { get; set; }
}

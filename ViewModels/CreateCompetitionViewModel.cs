using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.ViewModels;

public class CreateCompetitionViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Competition name is required.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Competition name must be at least 3 characters long.")]
    [Display(Name = "Competition name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Season is required.")]
    [RegularExpression(@"^\d{4}/\d{4}$", ErrorMessage = "Season must be in format 2025/2026.")]
    [Display(Name = "Season")]
    public string Season { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    [Display(Name = "Start date")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "End date is required.")]
    [Display(Name = "End date")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "City must be longer than 2 characters.")]
    [RegularExpression(@"^[^\d]+$", ErrorMessage = "City cannot contain numbers.")]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;
}

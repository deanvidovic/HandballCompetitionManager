using System.ComponentModel.DataAnnotations;
using HandballCompetitionManager.Models;

namespace HandballCompetitionManager.ViewModels;

public class CreatePlayerViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "First name must be at least 2 characters long.")]
    [RegularExpression(@"^[^\d]+$", ErrorMessage = "First name cannot contain numbers.")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Last name must be at least 2 characters long.")]
    [RegularExpression(@"^[^\d]+$", ErrorMessage = "Last name cannot contain numbers.")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birth date is required.")]
    [Display(Name = "Birth date")]
    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = "Jersey number is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Jersey number can contain only numbers.")]
    [Display(Name = "Jersey number")]
    public string JerseyNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Position is required.")]
    [Display(Name = "Position")]
    public PlayerPosition? Position { get; set; }

    [Required(ErrorMessage = "Team is required.")]
    [Display(Name = "Team")]
    public int? TeamId { get; set; }

    [Required(ErrorMessage = "Goals scored is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Goals scored can contain only numbers.")]
    [Display(Name = "Goals scored")]
    public string GoalsScored { get; set; } = "0";

    [Required(ErrorMessage = "Assists are required.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Assists can contain only numbers.")]
    [Display(Name = "Assists")]
    public string Assists { get; set; } = "0";
}

using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.ViewModels;

public class CreateTeamViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Team name is required.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Team name must be at least 3 characters long.")]
    [Display(Name = "Team name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Coach is required.")]
    [StringLength(120)]
    [Display(Name = "Coach")]
    public string CoachName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Home city is required.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Home city must be at least 3 characters long.")]
    [RegularExpression(@"^[^\d]+$", ErrorMessage = "Home city cannot contain numbers.")]
    [Display(Name = "Home city")]
    public string HomeCity { get; set; } = string.Empty;

    [Required(ErrorMessage = "Founded year is required.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Founded year can contain only numbers.")]
    [Display(Name = "Founded year")]
    public string FoundedYear { get; set; } = string.Empty;

    [Required(ErrorMessage = "Home arena is required.")]
    [StringLength(120, MinimumLength = 4, ErrorMessage = "Home arena must be at least 4 characters long.")]
    [Display(Name = "Home arena")]
    public string HomeArena { get; set; } = string.Empty;
}

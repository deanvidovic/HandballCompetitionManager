using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Dtos;

public record CompetitionDto(
    int Id,
    string Name,
    string Season,
    DateTime StartDate,
    DateTime EndDate,
    string City,
    IReadOnlyCollection<TeamSummaryDto> Teams,
    IReadOnlyCollection<GroupPhaseSummaryDto> Groups,
    IReadOnlyCollection<AppUserSummaryDto> Administrators);

public class CompetitionWriteDto
{
    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Season { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required, StringLength(80)]
    public string City { get; set; } = string.Empty;

    public List<int> TeamIds { get; set; } = new();
    public List<int> AdministratorIds { get; set; } = new();
}

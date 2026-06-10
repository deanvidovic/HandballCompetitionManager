using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Dtos;

public record TeamDto(
    int Id,
    string Name,
    string CoachName,
    string HomeCity,
    int FoundedYear,
    string HomeArena,
    IReadOnlyCollection<PlayerSummaryDto> Players,
    IReadOnlyCollection<CompetitionSummaryDto> Competitions);

public class TeamWriteDto
{
    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string CoachName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string HomeCity { get; set; } = string.Empty;

    [Range(1800, 2026)]
    public int FoundedYear { get; set; }

    [Required, StringLength(120)]
    public string HomeArena { get; set; } = string.Empty;

    public List<int> CompetitionIds { get; set; } = new();
    public List<int> GroupPhaseIds { get; set; } = new();
}

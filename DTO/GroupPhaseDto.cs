using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Dtos;

public record GroupPhaseDto(
    int Id,
    string Name,
    CompetitionSummaryDto? Competition,
    IReadOnlyCollection<TeamSummaryDto> Teams,
    IReadOnlyCollection<MatchSummaryDto> Matches);

public class GroupPhaseWriteDto
{
    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CompetitionId { get; set; }

    public List<int> TeamIds { get; set; } = new();
}

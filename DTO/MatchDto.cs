using System.ComponentModel.DataAnnotations;
using HandballCompetitionManager.Models;

namespace HandballCompetitionManager.Dtos;

public record MatchDto(
    int Id,
    CompetitionSummaryDto? Competition,
    GroupPhaseSummaryDto? GroupPhase,
    int RoundNumber,
    DateTime Kickoff,
    MatchTeamDto? HomeTeam,
    MatchTeamDto? AwayTeam,
    int HomeScore,
    int AwayScore,
    string MaintenanceHall,
    MatchStatus Status,
    string? ReportFilePath,
    string? ReportFileName);

public class MatchWriteDto
{
    [Range(1, int.MaxValue)]
    public int CompetitionId { get; set; }

    [Range(1, int.MaxValue)]
    public int GroupId { get; set; }

    [Range(1, int.MaxValue)]
    public int RoundNumber { get; set; }

    [Required]
    public DateTime Kickoff { get; set; }

    [Range(1, int.MaxValue)]
    public int HomeTeamId { get; set; }

    [Range(1, int.MaxValue)]
    public int AwayTeamId { get; set; }

    [Range(0, int.MaxValue)]
    public int HomeScore { get; set; }

    [Range(0, int.MaxValue)]
    public int AwayScore { get; set; }

    [StringLength(120)]
    public string MaintenanceHall { get; set; } = string.Empty;

    public MatchStatus Status { get; set; }
}

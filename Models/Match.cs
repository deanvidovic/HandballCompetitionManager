namespace HandballCompetitionManager.Models;

public class Match
{
    public int Id { get; set; }
    public int CompetitionId { get; set; }
    public int GroupId { get; set; }
    public int RoundNumber { get; set; }
    public DateTime Kickoff { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public string MaintenanceHall { get; set; } = string.Empty;
    public MatchStatus Status { get; set; }
}

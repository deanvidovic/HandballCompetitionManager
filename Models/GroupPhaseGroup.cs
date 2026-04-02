namespace HandballCompetitionManager.Models;

public class GroupPhaseGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CompetitionId { get; set; }
    public List<Team> Teams { get; set; } = new();
    public List<Match> Matches { get; set; } = new();
}

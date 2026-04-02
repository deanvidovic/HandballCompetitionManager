namespace HandballCompetitionManager.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Club { get; set; } = string.Empty;
    public string CoachName { get; set; } = string.Empty;
    public string HomeCity { get; set; } = string.Empty;
    public int FoundedYear { get; set; }
    public string HomeArena { get; set; } = string.Empty;

    // 1-N relation: one team has many players.
    public List<Player> Players { get; set; } = new();

    // N-N relation with competitions.
    public List<Competition> Competitions { get; set; } = new();
}

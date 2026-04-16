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
    public List<Player> Players { get; set; } = new();
    public List<Competition> Competitions { get; set; } = new();
}

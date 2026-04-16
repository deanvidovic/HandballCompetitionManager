namespace HandballCompetitionManager.Models;

public class Competition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string City { get; set; } = string.Empty;
    public List<Team> Teams { get; set; } = new();
    public List<GroupPhase> Groups { get; set; } = new();
    public List<AppUser> Administrators { get; set; } = new();
}

using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Models;

public class Competition
{
    public int CompetitionId { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(80)]
    public string Season { get; set; } = string.Empty;

    [StringLength(80)]
    public string Category { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public CompetitionStatus Status { get; set; }

    public int MaximumTeams { get; set; }

    public ICollection<Team> Teams { get; set; } = new List<Team>();

    public ICollection<Match> Matches { get; set; } = new List<Match>();
}

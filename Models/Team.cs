using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Models;

public class Team
{
    public int TeamId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(80)]
    public string City { get; set; } = string.Empty;

    [StringLength(80)]
    public string Country { get; set; } = string.Empty;

    [StringLength(100)]
    public string CoachName { get; set; } = string.Empty;

    [StringLength(120)]
    public string ContactEmail { get; set; } = string.Empty;

    public DateTime FoundedDate { get; set; }

    public ICollection<Player> Players { get; set; } = new List<Player>();

    public ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public ICollection<Match> HomeMatches { get; set; } = new List<Match>();

    public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
}

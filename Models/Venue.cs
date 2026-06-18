using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Models;

public class Venue
{
    public int VenueId { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(160)]
    public string Address { get; set; } = string.Empty;

    [StringLength(80)]
    public string City { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public bool HasElectronicScoreboard { get; set; }

    public ICollection<Match> Matches { get; set; } = new List<Match>();
}

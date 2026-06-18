using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Models;

public class Referee
{
    public int RefereeId { get; set; }

    [Required]
    [StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [StringLength(40)]
    public string LicenseNumber { get; set; } = string.Empty;

    public DateTime LicenseValidUntil { get; set; }

    public ICollection<Match> Matches { get; set; } = new List<Match>();
}

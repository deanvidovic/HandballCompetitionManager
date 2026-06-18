using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Models;

public class Player
{
    public int PlayerId { get; set; }

    [Required]
    [StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public int ShirtNumber { get; set; }

    [StringLength(40)]
    public string Position { get; set; } = string.Empty;

    public bool IsCaptain { get; set; }

    public int TeamId { get; set; }

    public Team? Team { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandballCompetitionManager.Models;

public class Team
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CoachName { get; set; } = string.Empty;
    public string HomeCity { get; set; } = string.Empty;
    public int FoundedYear { get; set; }
    public string HomeArena { get; set; } = string.Empty;
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
    public virtual ICollection<GroupPhase> GroupPhases { get; set; } = new List<GroupPhase>();
}

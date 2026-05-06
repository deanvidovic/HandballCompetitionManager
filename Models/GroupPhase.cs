using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandballCompetitionManager.Models;

public class GroupPhase
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    [ForeignKey("Competition")]
    public int CompetitionId { get; set; }
    public virtual Competition? Competition { get; set; }
    
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}

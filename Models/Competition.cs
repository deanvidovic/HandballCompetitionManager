using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandballCompetitionManager.Models;

public class Competition
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string City { get; set; } = string.Empty;
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    public virtual ICollection<GroupPhase> Groups { get; set; } = new List<GroupPhase>();
    public virtual ICollection<AppUser> Administrators { get; set; } = new List<AppUser>();
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandballCompetitionManager.Models;

public class Player
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public int JerseyNumber { get; set; }
    public PlayerPosition Position { get; set; }
    
    [ForeignKey("Team")]
    public int TeamId { get; set; }
    public virtual Team? Team { get; set; }
    
    public int GoalsScored { get; set; }
    public int Assists { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandballCompetitionManager.Models;

public class Match
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Competition")]
    public int CompetitionId { get; set; }
    public virtual Competition? Competition { get; set; }
    
    [ForeignKey("GroupPhase")]
    public int GroupId { get; set; }
    public virtual GroupPhase? GroupPhase { get; set; }
    
    public int RoundNumber { get; set; }
    public DateTime Kickoff { get; set; }
    
    [ForeignKey("HomeTeam")]
    public int HomeTeamId { get; set; }
    public virtual Team? HomeTeam { get; set; }
    
    [ForeignKey("AwayTeam")]
    public int AwayTeamId { get; set; }
    public virtual Team? AwayTeam { get; set; }
    
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public string MaintenanceHall { get; set; } = string.Empty;
    public MatchStatus Status { get; set; }
}

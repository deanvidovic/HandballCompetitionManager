using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.Models;

public class Match
{
    public int MatchId { get; set; }

    public DateTime ScheduledAt { get; set; }

    [StringLength(40)]
    public string RoundName { get; set; } = string.Empty;

    public MatchStatus Status { get; set; }

    public int? HomeScore { get; set; }

    public int? AwayScore { get; set; }

    public int CompetitionId { get; set; }

    public Competition? Competition { get; set; }

    public int HomeTeamId { get; set; }

    public Team? HomeTeam { get; set; }

    public int AwayTeamId { get; set; }

    public Team? AwayTeam { get; set; }

    public int VenueId { get; set; }

    public Venue? Venue { get; set; }

    public ICollection<Referee> Referees { get; set; } = new List<Referee>();
}

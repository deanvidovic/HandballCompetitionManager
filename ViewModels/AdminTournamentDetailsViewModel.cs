namespace HandballCompetitionManager.ViewModels;

public sealed class AdminTournamentDetailsViewModel
{
    public TournamentDetailsViewModel Details { get; init; } = new();

    public int UpcomingMatches => Details.Statistics.TotalMatches - Details.Statistics.CompletedMatches;

    public string SetupStatus => Details.Tournament.Status == "Upcoming" ? "Setup In Progress" : "Setup Complete";

    public string TeamsRegisteredStatus => Details.Teams.Count == Details.Tournament.TeamCount ? "Teams Registered" : "Registration Open";

    public string BracketStatus => Details.BracketRounds.Any(round => round.Matches.Count > 0) ? "Bracket Ready" : "Bracket Pending";

    public string ResultsStatus => UpcomingMatches > 0 ? "Results Pending" : "Results Confirmed";

    public string PhaseProgress => $"{Details.Tournament.CurrentPhase} Active";
}

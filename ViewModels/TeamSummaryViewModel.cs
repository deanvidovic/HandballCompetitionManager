namespace HandballCompetitionManager.ViewModels;

public sealed class TeamSummaryViewModel
{
    public int Id { get; init; }

    public int TournamentId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string Country { get; init; } = string.Empty;

    public string CoachName { get; init; } = string.Empty;
}

namespace HandballCompetitionManager.ViewModels;

public sealed class PlayerSummaryViewModel
{
    public int Id { get; init; }

    public int TeamId { get; init; }

    public string FullName { get; init; } = string.Empty;

    public int ShirtNumber { get; init; }

    public string Position { get; init; } = string.Empty;

    public bool IsCaptain { get; init; }
}

using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Interfaces;

public interface IMatchRepository
{
    Task<IReadOnlyList<MatchSummaryViewModel>> GetAllAsync();

    Task<IReadOnlyList<MatchSummaryViewModel>> GetByTournamentIdAsync(int tournamentId);

    Task<MatchSummaryViewModel?> GetByIdAsync(int id);

    Task<IReadOnlyList<MatchEventSummaryViewModel>> GetEventsByMatchIdAsync(int matchId);
}

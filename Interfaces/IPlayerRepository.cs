using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Interfaces;

public interface IPlayerRepository
{
    Task<IReadOnlyList<PlayerSummaryViewModel>> GetAllAsync();

    Task<IReadOnlyList<PlayerSummaryViewModel>> GetByTeamIdAsync(int teamId);

    Task<IReadOnlyList<PlayerStatisticViewModel>> GetStatisticsByTournamentIdAsync(int tournamentId);

    Task<IReadOnlyList<PlayerMatchPerformanceViewModel>> GetMatchPerformancesByPlayerIdAsync(int playerId, int tournamentId);

    Task<IReadOnlyList<MatchPlayerPerformanceViewModel>> GetMatchPerformancesByMatchIdAsync(int matchId);

    Task<PlayerSummaryViewModel?> GetByIdAsync(int id);
}

using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Interfaces;

public interface ITournamentRepository
{
    Task<IReadOnlyList<TournamentCardViewModel>> GetAllAsync();

    Task<TournamentCardViewModel?> GetByIdAsync(int id);

    Task<IReadOnlyList<GroupStandingViewModel>> GetGroupStandingsAsync(int tournamentId);

    Task<TournamentStatisticViewModel?> GetStatisticsAsync(int tournamentId);
}

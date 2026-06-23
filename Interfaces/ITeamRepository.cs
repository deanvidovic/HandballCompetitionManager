using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Interfaces;

public interface ITeamRepository
{
    Task<IReadOnlyList<TeamSummaryViewModel>> GetAllAsync();

    Task<IReadOnlyList<TeamSummaryViewModel>> GetByTournamentIdAsync(int tournamentId);

    Task<TeamSummaryViewModel?> GetByIdAsync(int id);
}

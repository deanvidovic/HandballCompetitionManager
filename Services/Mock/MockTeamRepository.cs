using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Services.Mock;

public sealed class MockTeamRepository : ITeamRepository
{
    public Task<IReadOnlyList<TeamSummaryViewModel>> GetAllAsync()
    {
        return Task.FromResult(MockDataStore.Teams);
    }

    public Task<IReadOnlyList<TeamSummaryViewModel>> GetByTournamentIdAsync(int tournamentId)
    {
        IReadOnlyList<TeamSummaryViewModel> teams = MockDataStore.Teams
            .Where(team => team.TournamentId == tournamentId)
            .ToList();

        return Task.FromResult(teams);
    }

    public Task<TeamSummaryViewModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(MockDataStore.Teams.FirstOrDefault(team => team.Id == id));
    }
}

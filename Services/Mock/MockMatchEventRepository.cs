using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Services.Mock;

public sealed class MockMatchEventRepository : IMatchEventRepository
{
    public Task<IReadOnlyList<MatchEventSummaryViewModel>> GetAllAsync()
    {
        return Task.FromResult(MockDataStore.MatchEvents);
    }

    public Task<IReadOnlyList<MatchEventSummaryViewModel>> GetByMatchIdAsync(int matchId)
    {
        IReadOnlyList<MatchEventSummaryViewModel> matchEvents = MockDataStore.MatchEvents
            .Where(matchEvent => matchEvent.MatchId == matchId)
            .ToList();

        return Task.FromResult(matchEvents);
    }
}

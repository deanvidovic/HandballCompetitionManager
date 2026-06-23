using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Services.Mock;

public sealed class MockMatchRepository : IMatchRepository
{
    public Task<IReadOnlyList<MatchSummaryViewModel>> GetAllAsync()
    {
        return Task.FromResult(MockDataStore.Matches);
    }

    public Task<IReadOnlyList<MatchSummaryViewModel>> GetByTournamentIdAsync(int tournamentId)
    {
        IReadOnlyList<MatchSummaryViewModel> matches = MockDataStore.Matches
            .Where(match => match.TournamentId == tournamentId)
            .ToList();

        return Task.FromResult(matches);
    }

    public Task<MatchSummaryViewModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(MockDataStore.Matches.FirstOrDefault(match => match.Id == id));
    }

    public Task<IReadOnlyList<MatchEventSummaryViewModel>> GetEventsByMatchIdAsync(int matchId)
    {
        IReadOnlyList<MatchEventSummaryViewModel> events = MockDataStore.MatchEvents
            .Where(matchEvent => matchEvent.MatchId == matchId)
            .OrderBy(matchEvent => matchEvent.Minute)
            .ThenBy(matchEvent => matchEvent.Id)
            .ToList();

        return Task.FromResult(events);
    }
}

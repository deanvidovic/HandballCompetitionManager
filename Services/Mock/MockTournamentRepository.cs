using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Services.Mock;

public sealed class MockTournamentRepository : ITournamentRepository
{
    public Task<IReadOnlyList<TournamentCardViewModel>> GetAllAsync()
    {
        return Task.FromResult(MockDataStore.Tournaments);
    }

    public Task<TournamentCardViewModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(MockDataStore.Tournaments.FirstOrDefault(tournament => tournament.Id == id));
    }

    public Task<IReadOnlyList<GroupStandingViewModel>> GetGroupStandingsAsync(int tournamentId)
    {
        IReadOnlyList<GroupStandingViewModel> standings = MockDataStore.GroupStandings
            .Where(standing => standing.TournamentId == tournamentId)
            .ToList();

        return Task.FromResult(standings);
    }

    public Task<TournamentStatisticViewModel?> GetStatisticsAsync(int tournamentId)
    {
        TournamentStatisticViewModel? statistics = MockDataStore.Tournaments.Any(tournament => tournament.Id == tournamentId)
            ? new TournamentStatisticViewModel
            {
                TournamentId = tournamentId,
                TotalTeams = MockDataStore.Teams.Count(team => team.TournamentId == tournamentId),
                TotalPlayers = MockDataStore.Teams
                    .Where(team => team.TournamentId == tournamentId)
                    .Join(MockDataStore.Players, team => team.Id, player => player.TeamId, (_, player) => player)
                    .Count(),
                TotalMatches = MockDataStore.Matches.Count(match => match.TournamentId == tournamentId),
                CompletedMatches = MockDataStore.Matches.Count(match => match.TournamentId == tournamentId && match.Status == "Completed"),
                TotalGoals = MockDataStore.Matches
                    .Where(match => match.TournamentId == tournamentId && match.HomeScore.HasValue && match.AwayScore.HasValue)
                    .Sum(match => match.HomeScore!.Value + match.AwayScore!.Value)
            }
            : null;

        return Task.FromResult(statistics);
    }
}

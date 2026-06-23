using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Services.Mock;

public sealed class MockPlayerRepository : IPlayerRepository
{
    public Task<IReadOnlyList<PlayerSummaryViewModel>> GetAllAsync()
    {
        return Task.FromResult(MockDataStore.Players);
    }

    public Task<IReadOnlyList<PlayerSummaryViewModel>> GetByTeamIdAsync(int teamId)
    {
        IReadOnlyList<PlayerSummaryViewModel> players = MockDataStore.Players
            .Where(player => player.TeamId == teamId)
            .ToList();

        return Task.FromResult(players);
    }

    public Task<IReadOnlyList<PlayerStatisticViewModel>> GetStatisticsByTournamentIdAsync(int tournamentId)
    {
        IReadOnlyList<PlayerStatisticViewModel> statistics = MockDataStore.PlayerStatistics
            .Where(statistic => statistic.TournamentId == tournamentId)
            .ToList();

        return Task.FromResult(statistics);
    }

    public Task<IReadOnlyList<PlayerMatchPerformanceViewModel>> GetMatchPerformancesByPlayerIdAsync(int playerId, int tournamentId)
    {
        PlayerSummaryViewModel? player = MockDataStore.Players.FirstOrDefault(item => item.Id == playerId);

        if (player is null)
        {
            return Task.FromResult<IReadOnlyList<PlayerMatchPerformanceViewModel>>([]);
        }

        IReadOnlyList<PlayerMatchPerformanceViewModel> performances = MockDataStore.Matches
            .Where(match => match.TournamentId == tournamentId && match.Status == "Completed")
            .Where(match => match.HomeTeamId == player.TeamId || match.AwayTeamId == player.TeamId)
            .Select(match =>
            {
                IReadOnlyList<MatchEventSummaryViewModel> playerEvents = MockDataStore.MatchEvents
                    .Where(matchEvent => matchEvent.MatchId == match.Id && matchEvent.PlayerId == playerId)
                    .ToList();

                return new PlayerMatchPerformanceViewModel
                {
                    MatchId = match.Id,
                    MatchDate = match.ScheduledAt,
                    OpponentName = match.HomeTeamId == player.TeamId ? match.AwayTeamName : match.HomeTeamName,
                    Goals = playerEvents.Count(matchEvent => matchEvent.EventType == "Goal"),
                    YellowCards = playerEvents.Count(matchEvent => matchEvent.EventType == "Yellow Card"),
                    RedCards = playerEvents.Count(matchEvent => matchEvent.EventType == "Red Card"),
                    TwoMinuteSuspensions = playerEvents.Count(matchEvent => matchEvent.EventType == "Two-Minute Suspension")
                };
            })
            .ToList();

        return Task.FromResult(performances);
    }

    public Task<IReadOnlyList<MatchPlayerPerformanceViewModel>> GetMatchPerformancesByMatchIdAsync(int matchId)
    {
        MatchSummaryViewModel? match = MockDataStore.Matches.FirstOrDefault(item => item.Id == matchId);

        if (match is null)
        {
            return Task.FromResult<IReadOnlyList<MatchPlayerPerformanceViewModel>>([]);
        }

        int[] teamIds = [match.HomeTeamId, match.AwayTeamId];

        IReadOnlyList<MatchPlayerPerformanceViewModel> performances = MockDataStore.Players
            .Where(player => teamIds.Contains(player.TeamId))
            .Join(MockDataStore.Teams, player => player.TeamId, team => team.Id, (player, team) => new { player, team })
            .Select(item =>
            {
                IReadOnlyList<MatchEventSummaryViewModel> playerEvents = MockDataStore.MatchEvents
                    .Where(matchEvent => matchEvent.MatchId == matchId && matchEvent.PlayerId == item.player.Id)
                    .ToList();

                return new MatchPlayerPerformanceViewModel
                {
                    PlayerId = item.player.Id,
                    TeamId = item.team.Id,
                    PlayerName = item.player.FullName,
                    TeamName = item.team.Name,
                    Goals = playerEvents.Count(matchEvent => matchEvent.EventType == "Goal"),
                    YellowCards = playerEvents.Count(matchEvent => matchEvent.EventType == "Yellow Card"),
                    RedCards = playerEvents.Count(matchEvent => matchEvent.EventType == "Red Card"),
                    TwoMinuteSuspensions = playerEvents.Count(matchEvent => matchEvent.EventType == "Two-Minute Suspension")
                };
            })
            .OrderBy(performance => performance.TeamName)
            .ThenBy(performance => performance.PlayerName)
            .ToList();

        return Task.FromResult(performances);
    }

    public Task<PlayerSummaryViewModel?> GetByIdAsync(int id)
    {
        return Task.FromResult(MockDataStore.Players.FirstOrDefault(player => player.Id == id));
    }
}

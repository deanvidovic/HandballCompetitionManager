using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers;

public class PlayersController : Controller
{
    private readonly IPlayerRepository playerRepository;
    private readonly ITeamRepository teamRepository;
    private readonly IMatchRepository matchRepository;
    private readonly ITournamentRepository tournamentRepository;

    public PlayersController(
        IPlayerRepository playerRepository,
        ITeamRepository teamRepository,
        IMatchRepository matchRepository,
        ITournamentRepository tournamentRepository)
    {
        this.playerRepository = playerRepository;
        this.teamRepository = teamRepository;
        this.matchRepository = matchRepository;
        this.tournamentRepository = tournamentRepository;
    }

    public async Task<IActionResult> Details(int id)
    {
        PlayerSummaryViewModel? player = await playerRepository.GetByIdAsync(id);

        if (player is null)
        {
            return NotFound();
        }

        TeamSummaryViewModel? team = await teamRepository.GetByIdAsync(player.TeamId);

        if (team is null)
        {
            return NotFound();
        }

        TournamentCardViewModel? tournament = await tournamentRepository.GetByIdAsync(team.TournamentId);

        if (tournament is null)
        {
            return NotFound();
        }

        IReadOnlyList<MatchSummaryViewModel> tournamentMatches = await matchRepository.GetByTournamentIdAsync(tournament.Id);
        IReadOnlyList<PlayerStatisticViewModel> tournamentStatistics = await playerRepository.GetStatisticsByTournamentIdAsync(tournament.Id);
        IReadOnlyList<PlayerMatchPerformanceViewModel> matchPerformances = await playerRepository.GetMatchPerformancesByPlayerIdAsync(player.Id, tournament.Id);
        IReadOnlyList<GroupStandingViewModel> standings = await tournamentRepository.GetGroupStandingsAsync(tournament.Id);

        PlayerStatisticViewModel statistics = tournamentStatistics.FirstOrDefault(statistic => statistic.PlayerId == player.Id)
            ?? new PlayerStatisticViewModel
            {
                TournamentId = tournament.Id,
                PlayerId = player.Id,
                TeamId = team.Id,
                PlayerName = player.FullName,
                TeamName = team.Name
            };

        IReadOnlyList<MatchSummaryViewModel> teamMatches = tournamentMatches
            .Where(match => match.HomeTeamId == team.Id || match.AwayTeamId == team.Id)
            .ToList();

        PlayerDetailsViewModel viewModel = new()
        {
            Player = player,
            Team = team,
            Tournament = tournament,
            Statistics = statistics,
            Performance = BuildPerformance(statistics, matchPerformances),
            TeamPerformance = BuildTeamPerformance(team.Id, teamMatches),
            Description = $"{player.FullName} wears number {player.ShirtNumber} for {team.Name} and contributes from {player.Position} during {tournament.Name}.",
            AgeDisplay = "Not available",
            TournamentPosition = BuildTournamentPosition(team.Id, standings),
            MatchPerformances = matchPerformances.OrderBy(match => match.MatchDate).ToList(),
            Rankings = BuildRankings(statistics, tournamentStatistics)
        };

        return View(viewModel);
    }

    private static PlayerTournamentPerformanceViewModel BuildPerformance(
        PlayerStatisticViewModel statistics,
        IReadOnlyList<PlayerMatchPerformanceViewModel> matchPerformances)
    {
        return new PlayerTournamentPerformanceViewModel
        {
            Goals = statistics.Goals,
            Assists = 0,
            MatchesPlayed = matchPerformances.Count,
            YellowCards = statistics.YellowCards,
            RedCards = statistics.RedCards,
            TwoMinuteSuspensions = statistics.TwoMinuteSuspensions
        };
    }

    private static TeamPerformanceViewModel BuildTeamPerformance(int teamId, IReadOnlyList<MatchSummaryViewModel> matches)
    {
        IReadOnlyList<MatchSummaryViewModel> completedMatches = matches
            .Where(match => match.Status == "Completed")
            .ToList();

        int wins = completedMatches.Count(match => IsWinner(match, teamId));
        int draws = completedMatches.Count(match => match.HomeScore == match.AwayScore);
        int losses = completedMatches.Count - wins - draws;
        int goalsScored = completedMatches.Sum(match => match.HomeTeamId == teamId ? match.HomeScore ?? 0 : match.AwayScore ?? 0);
        int goalsConceded = completedMatches.Sum(match => match.HomeTeamId == teamId ? match.AwayScore ?? 0 : match.HomeScore ?? 0);

        return new TeamPerformanceViewModel
        {
            MatchesPlayed = completedMatches.Count,
            Wins = wins,
            Draws = draws,
            Losses = losses,
            GoalsScored = goalsScored,
            GoalsConceded = goalsConceded
        };
    }

    private static bool IsWinner(MatchSummaryViewModel match, int teamId)
    {
        if (!match.HomeScore.HasValue || !match.AwayScore.HasValue || match.HomeScore == match.AwayScore)
        {
            return false;
        }

        return match.HomeTeamId == teamId
            ? match.HomeScore > match.AwayScore
            : match.AwayTeamId == teamId && match.AwayScore > match.HomeScore;
    }

    private static string BuildTournamentPosition(int teamId, IReadOnlyList<GroupStandingViewModel> standings)
    {
        GroupStandingViewModel? standing = standings.FirstOrDefault(item => item.TeamId == teamId);

        if (standing is null)
        {
            return "Not ranked";
        }

        int position = standings
            .Where(item => item.GroupName == standing.GroupName)
            .OrderByDescending(item => item.Points)
            .ThenByDescending(item => item.GoalsFor - item.GoalsAgainst)
            .ThenBy(item => item.TeamName)
            .Select((item, index) => new { item.TeamId, Position = index + 1 })
            .First(item => item.TeamId == teamId)
            .Position;

        return $"{position} in {standing.GroupName}";
    }

    private static IReadOnlyList<PlayerRankingViewModel> BuildRankings(
        PlayerStatisticViewModel playerStatistics,
        IReadOnlyList<PlayerStatisticViewModel> tournamentStatistics)
    {
        return
        [
            BuildRanking("Goal scorer ranking", playerStatistics.PlayerId, tournamentStatistics, statistic => statistic.Goals),
            BuildRanking("Yellow card ranking", playerStatistics.PlayerId, tournamentStatistics, statistic => statistic.YellowCards),
            BuildRanking("Red card ranking", playerStatistics.PlayerId, tournamentStatistics, statistic => statistic.RedCards),
            BuildRanking("Two-minute suspension ranking", playerStatistics.PlayerId, tournamentStatistics, statistic => statistic.TwoMinuteSuspensions)
        ];
    }

    private static PlayerRankingViewModel BuildRanking(
        string label,
        int playerId,
        IReadOnlyList<PlayerStatisticViewModel> statistics,
        Func<PlayerStatisticViewModel, int> valueSelector)
    {
        var rankedPlayers = statistics
            .OrderByDescending(valueSelector)
            .ThenBy(statistic => statistic.PlayerName)
            .Select((statistic, index) => new { statistic.PlayerId, Rank = index + 1, Value = valueSelector(statistic) })
            .ToList();

        var playerRank = rankedPlayers.FirstOrDefault(statistic => statistic.PlayerId == playerId);

        return new PlayerRankingViewModel
        {
            Label = label,
            Rank = playerRank?.Rank ?? 0,
            Value = playerRank?.Value ?? 0
        };
    }
}

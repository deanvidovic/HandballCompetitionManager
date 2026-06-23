using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers;

public class TeamsController : Controller
{
    private readonly ITeamRepository teamRepository;
    private readonly IPlayerRepository playerRepository;
    private readonly IMatchRepository matchRepository;
    private readonly ITournamentRepository tournamentRepository;

    public TeamsController(
        ITeamRepository teamRepository,
        IPlayerRepository playerRepository,
        IMatchRepository matchRepository,
        ITournamentRepository tournamentRepository)
    {
        this.teamRepository = teamRepository;
        this.playerRepository = playerRepository;
        this.matchRepository = matchRepository;
        this.tournamentRepository = tournamentRepository;
    }

    public async Task<IActionResult> Details(int id)
    {
        TeamSummaryViewModel? team = await teamRepository.GetByIdAsync(id);

        if (team is null)
        {
            return NotFound();
        }

        TournamentCardViewModel? tournament = await tournamentRepository.GetByIdAsync(team.TournamentId);

        if (tournament is null)
        {
            return NotFound();
        }

        IReadOnlyList<PlayerSummaryViewModel> players = await playerRepository.GetByTeamIdAsync(team.Id);
        IReadOnlyList<MatchSummaryViewModel> tournamentMatches = await matchRepository.GetByTournamentIdAsync(team.TournamentId);
        IReadOnlyList<PlayerStatisticViewModel> tournamentStatistics = await playerRepository.GetStatisticsByTournamentIdAsync(team.TournamentId);
        IReadOnlyList<GroupStandingViewModel> standings = await tournamentRepository.GetGroupStandingsAsync(team.TournamentId);

        IReadOnlyList<MatchSummaryViewModel> teamMatches = tournamentMatches
            .Where(match => match.HomeTeamId == team.Id || match.AwayTeamId == team.Id)
            .OrderBy(match => match.ScheduledAt)
            .ToList();

        IReadOnlyList<PlayerStatisticViewModel> teamStatistics = tournamentStatistics
            .Where(statistic => statistic.TeamId == team.Id)
            .ToList();

        TeamDetailsViewModel viewModel = new()
        {
            Team = team,
            Tournament = tournament,
            Performance = BuildPerformance(team.Id, teamMatches),
            Description = $"{team.Name} represents {team.City}, {team.Country} in {tournament.Name}, with {team.CoachName} guiding the squad through the current tournament stage.",
            GroupName = standings.FirstOrDefault(standing => standing.TeamId == team.Id)?.GroupName ?? "Not assigned",
            Roster = BuildRoster(players, teamStatistics),
            Matches = teamMatches,
            PlayerLeaderboards = BuildPlayerLeaderboards(teamStatistics)
        };

        return View(viewModel);
    }

    private static TeamPerformanceViewModel BuildPerformance(int teamId, IReadOnlyList<MatchSummaryViewModel> matches)
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

    private static IReadOnlyList<TeamRosterPlayerViewModel> BuildRoster(
        IReadOnlyList<PlayerSummaryViewModel> players,
        IReadOnlyList<PlayerStatisticViewModel> statistics)
    {
        return players
            .OrderBy(player => player.ShirtNumber)
            .Select(player =>
            {
                PlayerStatisticViewModel? statistic = statistics.FirstOrDefault(item => item.PlayerId == player.Id);

                return new TeamRosterPlayerViewModel
                {
                    Id = player.Id,
                    FullName = player.FullName,
                    ShirtNumber = player.ShirtNumber,
                    Position = player.Position,
                    Goals = statistic?.Goals ?? 0,
                    YellowCards = statistic?.YellowCards ?? 0,
                    RedCards = statistic?.RedCards ?? 0,
                    TwoMinuteSuspensions = statistic?.TwoMinuteSuspensions ?? 0
                };
            })
            .ToList();
    }

    private static IReadOnlyList<StatisticLeaderboardViewModel> BuildPlayerLeaderboards(IReadOnlyList<PlayerStatisticViewModel> statistics)
    {
        return
        [
            BuildLeaderboard("Top Scorers", "Goals", statistics.OrderByDescending(statistic => statistic.Goals), statistic => statistic.Goals),
            BuildLeaderboard("Most Two-Minute Suspensions", "Suspensions", statistics.OrderByDescending(statistic => statistic.TwoMinuteSuspensions), statistic => statistic.TwoMinuteSuspensions),
            BuildLeaderboard("Most Yellow Cards", "Yellow cards", statistics.OrderByDescending(statistic => statistic.YellowCards), statistic => statistic.YellowCards),
            BuildLeaderboard("Most Red Cards", "Red cards", statistics.OrderByDescending(statistic => statistic.RedCards), statistic => statistic.RedCards)
        ];
    }

    private static StatisticLeaderboardViewModel BuildLeaderboard(
        string title,
        string statLabel,
        IOrderedEnumerable<PlayerStatisticViewModel> orderedStatistics,
        Func<PlayerStatisticViewModel, int> valueSelector)
    {
        return new StatisticLeaderboardViewModel
        {
            Title = title,
            StatLabel = statLabel,
            Leaders = orderedStatistics
                .ThenBy(statistic => statistic.PlayerName)
                .Take(3)
                .Select((statistic, index) => new StatisticLeaderViewModel
                {
                    Rank = index + 1,
                    PlayerName = statistic.PlayerName,
                    TeamName = statistic.TeamName,
                    Value = valueSelector(statistic)
                })
                .ToList()
        };
    }
}

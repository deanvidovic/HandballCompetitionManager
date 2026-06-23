using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers;

public class TournamentsController : Controller
{
    private readonly ITournamentRepository tournamentRepository;
    private readonly ITeamRepository teamRepository;
    private readonly IPlayerRepository playerRepository;
    private readonly IMatchRepository matchRepository;

    public TournamentsController(
        ITournamentRepository tournamentRepository,
        ITeamRepository teamRepository,
        IPlayerRepository playerRepository,
        IMatchRepository matchRepository)
    {
        this.tournamentRepository = tournamentRepository;
        this.teamRepository = teamRepository;
        this.playerRepository = playerRepository;
        this.matchRepository = matchRepository;
    }

    public async Task<IActionResult> Index()
    {
        IReadOnlyList<TournamentCardViewModel> tournaments = await tournamentRepository.GetAllAsync();
        return View(tournaments);
    }

    public async Task<IActionResult> Details(int id)
    {
        TournamentCardViewModel? tournament = await tournamentRepository.GetByIdAsync(id);

        if (tournament is null)
        {
            return NotFound();
        }

        IReadOnlyList<TeamSummaryViewModel> teams = await teamRepository.GetByTournamentIdAsync(id);
        IReadOnlyList<GroupStandingViewModel> standings = await tournamentRepository.GetGroupStandingsAsync(id);
        IReadOnlyList<MatchSummaryViewModel> matches = await matchRepository.GetByTournamentIdAsync(id);
        IReadOnlyList<PlayerStatisticViewModel> playerStatistics = await playerRepository.GetStatisticsByTournamentIdAsync(id);
        TournamentStatisticViewModel statistics = await tournamentRepository.GetStatisticsAsync(id) ?? new TournamentStatisticViewModel { TournamentId = id };

        TournamentDetailsViewModel viewModel = new()
        {
            Tournament = tournament,
            Statistics = statistics,
            Teams = await BuildTeamDetailsAsync(teams, matches),
            Groups = BuildGroups(standings),
            BracketRounds = BuildBracketRounds(matches),
            GroupMatches = matches
                .Where(match => match.RoundName.StartsWith("Group", StringComparison.OrdinalIgnoreCase))
                .OrderBy(match => match.ScheduledAt)
                .ToList(),
            EliminationMatches = matches
                .Where(match => IsEliminationMatch(match))
                .OrderBy(match => match.ScheduledAt)
                .ToList(),
            RecentMatches = matches
                .OrderByDescending(match => match.ScheduledAt)
                .ToList(),
            PlayerLeaderboards = BuildPlayerLeaderboards(playerStatistics)
        };

        return View(viewModel);
    }

    private async Task<IReadOnlyList<TournamentTeamDetailsViewModel>> BuildTeamDetailsAsync(
        IReadOnlyList<TeamSummaryViewModel> teams,
        IReadOnlyList<MatchSummaryViewModel> matches)
    {
        List<TournamentTeamDetailsViewModel> teamDetails = [];

        foreach (TeamSummaryViewModel team in teams)
        {
            IReadOnlyList<PlayerSummaryViewModel> players = await playerRepository.GetByTeamIdAsync(team.Id);
            IReadOnlyList<MatchSummaryViewModel> completedMatches = matches
                .Where(match => match.Status == "Completed" && (match.HomeTeamId == team.Id || match.AwayTeamId == team.Id))
                .ToList();

            int wins = completedMatches.Count(match => IsWinner(match, team.Id));
            int draws = completedMatches.Count(match => match.HomeScore == match.AwayScore);
            int losses = completedMatches.Count - wins - draws;
            int goalsScored = completedMatches.Sum(match => match.HomeTeamId == team.Id ? match.HomeScore ?? 0 : match.AwayScore ?? 0);
            int goalsConceded = completedMatches.Sum(match => match.HomeTeamId == team.Id ? match.AwayScore ?? 0 : match.HomeScore ?? 0);

            teamDetails.Add(new TournamentTeamDetailsViewModel
            {
                Id = team.Id,
                Name = team.Name,
                City = team.City,
                CoachName = team.CoachName,
                PlayerCount = players.Count,
                Record = $"{wins}-{draws}-{losses}",
                GoalsScored = goalsScored,
                GoalsConceded = goalsConceded
            });
        }

        return teamDetails;
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

    private static IReadOnlyList<TournamentGroupViewModel> BuildGroups(IReadOnlyList<GroupStandingViewModel> standings)
    {
        return standings
            .GroupBy(standing => standing.GroupName)
            .Select(group => new TournamentGroupViewModel
            {
                Name = group.Key,
                Standings = group
                    .OrderByDescending(standing => standing.Points)
                    .ThenByDescending(standing => standing.GoalsFor - standing.GoalsAgainst)
                    .Select((standing, index) => new GroupStandingRowViewModel
                    {
                        Position = index + 1,
                        Standing = standing
                    })
                    .ToList()
            })
            .ToList();
    }

    private static IReadOnlyList<TournamentBracketRoundViewModel> BuildBracketRounds(IReadOnlyList<MatchSummaryViewModel> matches)
    {
        string[] roundNames = ["Quarter-final", "Semifinal", "Final"];

        return roundNames
            .Select(roundName => new TournamentBracketRoundViewModel
            {
                Name = roundName,
                Matches = matches
                    .Where(match => string.Equals(match.RoundName, roundName, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(match => match.ScheduledAt)
                    .ToList()
            })
            .ToList();
    }

    private static bool IsEliminationMatch(MatchSummaryViewModel match)
    {
        return string.Equals(match.RoundName, "Quarter-final", StringComparison.OrdinalIgnoreCase)
            || string.Equals(match.RoundName, "Semifinal", StringComparison.OrdinalIgnoreCase)
            || string.Equals(match.RoundName, "Final", StringComparison.OrdinalIgnoreCase);
    }

    private static IReadOnlyList<StatisticLeaderboardViewModel> BuildPlayerLeaderboards(IReadOnlyList<PlayerStatisticViewModel> statistics)
    {
        return
        [
            BuildLeaderboard("Top Scorers", "Goals", statistics.OrderByDescending(statistic => statistic.Goals), statistic => statistic.Goals),
            BuildLeaderboard("Most Red Cards", "Red cards", statistics.OrderByDescending(statistic => statistic.RedCards), statistic => statistic.RedCards),
            BuildLeaderboard("Most Two-Minute Suspensions", "Suspensions", statistics.OrderByDescending(statistic => statistic.TwoMinuteSuspensions), statistic => statistic.TwoMinuteSuspensions),
            BuildLeaderboard("Most Yellow Cards", "Yellow cards", statistics.OrderByDescending(statistic => statistic.YellowCards), statistic => statistic.YellowCards)
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
                    PlayerId = statistic.PlayerId,
                    PlayerName = statistic.PlayerName,
                    TeamName = statistic.TeamName,
                    Value = valueSelector(statistic)
                })
                .ToList()
        };
    }
}

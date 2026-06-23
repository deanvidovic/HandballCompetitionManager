using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers;

public class MatchesController : Controller
{
    private readonly IMatchRepository matchRepository;
    private readonly ITeamRepository teamRepository;
    private readonly IPlayerRepository playerRepository;
    private readonly ITournamentRepository tournamentRepository;

    public MatchesController(
        IMatchRepository matchRepository,
        ITeamRepository teamRepository,
        IPlayerRepository playerRepository,
        ITournamentRepository tournamentRepository)
    {
        this.matchRepository = matchRepository;
        this.teamRepository = teamRepository;
        this.playerRepository = playerRepository;
        this.tournamentRepository = tournamentRepository;
    }

    public async Task<IActionResult> Details(int id)
    {
        MatchSummaryViewModel? match = await matchRepository.GetByIdAsync(id);

        if (match is null || match.HomeTeamId == 0 || match.AwayTeamId == 0)
        {
            return NotFound();
        }

        TournamentCardViewModel? tournament = await tournamentRepository.GetByIdAsync(match.TournamentId);
        TeamSummaryViewModel? homeTeam = await teamRepository.GetByIdAsync(match.HomeTeamId);
        TeamSummaryViewModel? awayTeam = await teamRepository.GetByIdAsync(match.AwayTeamId);

        if (tournament is null || homeTeam is null || awayTeam is null)
        {
            return NotFound();
        }

        IReadOnlyList<MatchEventSummaryViewModel> events = await matchRepository.GetEventsByMatchIdAsync(match.Id);
        IReadOnlyList<MatchPlayerPerformanceViewModel> playerPerformances = await playerRepository.GetMatchPerformancesByMatchIdAsync(match.Id);

        MatchDetailsViewModel viewModel = new()
        {
            Match = match,
            Tournament = tournament,
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            Events = events,
            PlayerPerformances = playerPerformances,
            Totals = BuildTotals(match, events),
            HomeComparison = BuildTeamComparison(homeTeam, match, events, true),
            AwayComparison = BuildTeamComparison(awayTeam, match, events, false),
            TopPerformers = BuildTopPerformers(playerPerformances)
        };

        return View(viewModel);
    }

    private static MatchTotalsViewModel BuildTotals(MatchSummaryViewModel match, IReadOnlyList<MatchEventSummaryViewModel> events)
    {
        return new MatchTotalsViewModel
        {
            TotalGoals = match.HomeScore.HasValue && match.AwayScore.HasValue
                ? match.HomeScore.Value + match.AwayScore.Value
                : events.Count(matchEvent => matchEvent.EventType == "Goal"),
            YellowCards = events.Count(matchEvent => matchEvent.EventType == "Yellow Card"),
            RedCards = events.Count(matchEvent => matchEvent.EventType == "Red Card"),
            TwoMinuteSuspensions = events.Count(matchEvent => matchEvent.EventType == "Two-Minute Suspension")
        };
    }

    private static TeamMatchComparisonViewModel BuildTeamComparison(
        TeamSummaryViewModel team,
        MatchSummaryViewModel match,
        IReadOnlyList<MatchEventSummaryViewModel> events,
        bool isHomeTeam)
    {
        return new TeamMatchComparisonViewModel
        {
            TeamId = team.Id,
            TeamName = team.Name,
            Goals = isHomeTeam ? match.HomeScore ?? 0 : match.AwayScore ?? 0,
            YellowCards = events.Count(matchEvent => matchEvent.TeamId == team.Id && matchEvent.EventType == "Yellow Card"),
            RedCards = events.Count(matchEvent => matchEvent.TeamId == team.Id && matchEvent.EventType == "Red Card"),
            TwoMinuteSuspensions = events.Count(matchEvent => matchEvent.TeamId == team.Id && matchEvent.EventType == "Two-Minute Suspension")
        };
    }

    private static IReadOnlyList<MatchTopPerformerViewModel> BuildTopPerformers(IReadOnlyList<MatchPlayerPerformanceViewModel> performances)
    {
        MatchPlayerPerformanceViewModel? topScorer = performances
            .OrderByDescending(performance => performance.Goals)
            .ThenBy(performance => performance.PlayerName)
            .FirstOrDefault();

        MatchPlayerPerformanceViewModel? disciplined = performances
            .Where(performance => performance.DisciplinaryTotal == 0)
            .OrderByDescending(performance => performance.Goals)
            .ThenBy(performance => performance.PlayerName)
            .FirstOrDefault();

        MatchPlayerPerformanceViewModel? penalized = performances
            .OrderByDescending(performance => performance.DisciplinaryTotal)
            .ThenBy(performance => performance.PlayerName)
            .FirstOrDefault();

        return
        [
            BuildTopPerformer("Top Goal Scorer", topScorer, topScorer is null ? "0 goals" : $"{topScorer.Goals} goals"),
            BuildTopPerformer("Most Disciplined Player", disciplined, "No cards or suspensions"),
            BuildTopPerformer("Most Penalized Player", penalized, penalized is null ? "0 events" : $"{penalized.DisciplinaryTotal} events")
        ];
    }

    private static MatchTopPerformerViewModel BuildTopPerformer(
        string title,
        MatchPlayerPerformanceViewModel? performance,
        string statisticValue)
    {
        return new MatchTopPerformerViewModel
        {
            Title = title,
            PlayerId = performance?.PlayerId ?? 0,
            PlayerName = performance?.PlayerName ?? "Not available",
            TeamName = performance?.TeamName ?? "Not available",
            StatisticValue = statisticValue
        };
    }
}

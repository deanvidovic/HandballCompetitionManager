using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers;

public sealed class RefereeController : Controller
{
    private const int DemoRefereeUserId = 4;

    private readonly IMatchRepository matchRepository;
    private readonly ITournamentRepository tournamentRepository;
    private readonly ITeamRepository teamRepository;
    private readonly IUserRepository userRepository;

    public RefereeController(
        IMatchRepository matchRepository,
        ITournamentRepository tournamentRepository,
        ITeamRepository teamRepository,
        IUserRepository userRepository)
    {
        this.matchRepository = matchRepository;
        this.tournamentRepository = tournamentRepository;
        this.teamRepository = teamRepository;
        this.userRepository = userRepository;
    }

    public async Task<IActionResult> MyMatches()
    {
        UserSummaryViewModel? referee = await userRepository.GetByIdAsync(DemoRefereeUserId);
        IReadOnlyList<int> assignedMatchIds = await userRepository.GetAssignedMatchIdsByUserIdAsync(DemoRefereeUserId);
        IReadOnlyList<MatchSummaryViewModel> matches = await matchRepository.GetAllAsync();
        IReadOnlyList<TournamentCardViewModel> tournaments = await tournamentRepository.GetAllAsync();
        IReadOnlyList<TeamSummaryViewModel> teams = await teamRepository.GetAllAsync();

        RefereeMyMatchesViewModel viewModel = new()
        {
            Referee = referee,
            Matches = matches
                .Where(match => assignedMatchIds.Contains(match.Id))
                .OrderBy(match => match.ScheduledAt)
                .Select(match => BuildRefereeMatchRow(match, tournaments, teams))
                .ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Profile()
    {
        UserSummaryViewModel? referee = await userRepository.GetByIdAsync(DemoRefereeUserId);

        if (referee is null)
        {
            return View(new RefereeProfileViewModel());
        }

        IReadOnlyList<int> assignedMatchIds = await userRepository.GetAssignedMatchIdsByUserIdAsync(DemoRefereeUserId);
        IReadOnlyList<MatchSummaryViewModel> matches = await matchRepository.GetAllAsync();
        IReadOnlyList<MatchSummaryViewModel> assignedMatches = matches
            .Where(match => assignedMatchIds.Contains(match.Id))
            .ToList();

        RefereeProfileViewModel viewModel = new()
        {
            Referee = referee,
            AssignedMatches = assignedMatches.Count,
            UpcomingMatches = assignedMatches.Count(match =>
                !string.Equals(match.Status, "Completed", StringComparison.OrdinalIgnoreCase)
                && match.ScheduledAt >= DateTime.Today),
            CompletedMatches = assignedMatches.Count(match =>
                string.Equals(match.Status, "Completed", StringComparison.OrdinalIgnoreCase)),
            ConfirmedReports = referee.ConfirmedReports
        };

        return View(viewModel);
    }

    private static RefereeMatchRowViewModel BuildRefereeMatchRow(
        MatchSummaryViewModel match,
        IReadOnlyList<TournamentCardViewModel> tournaments,
        IReadOnlyList<TeamSummaryViewModel> teams)
    {
        string tournamentName = tournaments.FirstOrDefault(tournament => tournament.Id == match.TournamentId)?.Name ?? "Unknown Tournament";
        string homeTeamName = teams.FirstOrDefault(team => team.Id == match.HomeTeamId)?.Name ?? match.HomeTeamName;
        string awayTeamName = teams.FirstOrDefault(team => team.Id == match.AwayTeamId)?.Name ?? match.AwayTeamName;

        return new RefereeMatchRowViewModel
        {
            Id = match.Id,
            HomeTeamName = homeTeamName,
            AwayTeamName = awayTeamName,
            TournamentName = tournamentName,
            Phase = match.RoundName,
            ScheduledAt = match.ScheduledAt,
            Status = match.Status,
            VenueName = match.VenueName,
            Score = match.HomeScore.HasValue && match.AwayScore.HasValue
                ? $"{match.HomeScore}:{match.AwayScore}"
                : "Not played"
        };
    }
}

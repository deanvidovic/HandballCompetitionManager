using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers;

public sealed class AdminController : Controller
{
    private readonly ITournamentRepository tournamentRepository;
    private readonly ITeamRepository teamRepository;
    private readonly IMatchRepository matchRepository;
    private readonly IPlayerRepository playerRepository;
    private readonly IUserRepository userRepository;

    public AdminController(
        ITournamentRepository tournamentRepository,
        ITeamRepository teamRepository,
        IMatchRepository matchRepository,
        IPlayerRepository playerRepository,
        IUserRepository userRepository)
    {
        this.tournamentRepository = tournamentRepository;
        this.teamRepository = teamRepository;
        this.matchRepository = matchRepository;
        this.playerRepository = playerRepository;
        this.userRepository = userRepository;
    }

    public async Task<IActionResult> Tournaments()
    {
        IReadOnlyList<TournamentCardViewModel> tournaments = await tournamentRepository.GetAllAsync();

        AdminTournamentManagementViewModel viewModel = new()
        {
            Tournaments = tournaments
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Teams()
    {
        IReadOnlyList<TeamSummaryViewModel> teams = await teamRepository.GetAllAsync();
        IReadOnlyList<TournamentCardViewModel> tournaments = await tournamentRepository.GetAllAsync();
        IReadOnlyList<PlayerSummaryViewModel> players = await playerRepository.GetAllAsync();
        IReadOnlyList<MatchSummaryViewModel> matches = await matchRepository.GetAllAsync();

        AdminTeamManagementViewModel viewModel = new()
        {
            Teams = teams
                .OrderBy(team => team.Name)
                .Select(team => BuildAdminTeamRow(team, tournaments, players, matches))
                .ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Players()
    {
        IReadOnlyList<PlayerSummaryViewModel> players = await playerRepository.GetAllAsync();
        IReadOnlyList<TeamSummaryViewModel> teams = await teamRepository.GetAllAsync();
        IReadOnlyList<PlayerStatisticViewModel> playerStatistics = await BuildAllPlayerStatisticsAsync(teams);

        AdminPlayerManagementViewModel viewModel = new()
        {
            Players = players
                .OrderBy(player => teams.FirstOrDefault(team => team.Id == player.TeamId)?.Name ?? string.Empty)
                .ThenBy(player => player.ShirtNumber)
                .Select(player => BuildAdminPlayerRow(player, teams, playerStatistics))
                .ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Matches()
    {
        IReadOnlyList<MatchSummaryViewModel> matches = await matchRepository.GetAllAsync();
        IReadOnlyList<TournamentCardViewModel> tournaments = await tournamentRepository.GetAllAsync();

        AdminMatchManagementViewModel viewModel = new()
        {
            Matches = matches
                .OrderBy(match => match.ScheduledAt)
                .Select(match => BuildAdminMatchRow(match, tournaments))
                .ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Users()
    {
        IReadOnlyList<UserSummaryViewModel> users = await userRepository.GetAllAsync();

        AdminUserManagementViewModel viewModel = new()
        {
            Users = users
                .OrderBy(user => user.Role)
                .ThenBy(user => user.FullName)
                .ToList()
        };

        return View(viewModel);
    }

    [Route("Admin/Users/Details/{id:int}")]
    public async Task<IActionResult> UserDetails(int id)
    {
        UserSummaryViewModel? user = await userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        IReadOnlyList<TeamSummaryViewModel> teams = await teamRepository.GetAllAsync();
        IReadOnlyList<MatchSummaryViewModel> matches = await matchRepository.GetAllAsync();
        IReadOnlyList<int> assignedMatchIds = await userRepository.GetAssignedMatchIdsByUserIdAsync(id);

        AdminUserDetailsViewModel viewModel = new()
        {
            User = user,
            AssignedTeam = teams.FirstOrDefault(team => team.Id == user.AssignedTeamId),
            AssignedMatches = matches
                .Where(match => assignedMatchIds.Contains(match.Id))
                .OrderBy(match => match.ScheduledAt)
                .ToList()
        };

        return View(viewModel);
    }

    [Route("Admin/Matches/Details/{id:int}")]
    public async Task<IActionResult> MatchDetails(int id)
    {
        MatchSummaryViewModel? match = await matchRepository.GetByIdAsync(id);

        if (match is null || match.HomeTeamId <= 0 || match.AwayTeamId <= 0)
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
        UserSummaryViewModel? referee = await userRepository.GetAssignedRefereeByMatchIdAsync(match.Id);

        AdminMatchDetailsViewModel viewModel = new()
        {
            Match = match,
            Tournament = tournament,
            HomeTeam = homeTeam,
            AwayTeam = awayTeam,
            Referee = referee,
            Events = events,
            PlayerPerformances = playerPerformances,
            Totals = BuildMatchTotals(match, events),
            HomeComparison = BuildMatchComparison(homeTeam.Id, homeTeam.Name, match, events),
            AwayComparison = BuildMatchComparison(awayTeam.Id, awayTeam.Name, match, events)
        };

        return View(viewModel);
    }

    [Route("Admin/Players/Details/{id:int}")]
    public async Task<IActionResult> PlayerDetails(int id)
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

        IReadOnlyList<PlayerStatisticViewModel> tournamentStatistics = await playerRepository.GetStatisticsByTournamentIdAsync(team.TournamentId);
        IReadOnlyList<PlayerMatchPerformanceViewModel> matchPerformances = await playerRepository.GetMatchPerformancesByPlayerIdAsync(id, team.TournamentId);
        IReadOnlyList<MatchSummaryViewModel> tournamentMatches = await matchRepository.GetByTournamentIdAsync(team.TournamentId);
        IReadOnlyList<MatchSummaryViewModel> teamMatches = tournamentMatches
            .Where(match => match.HomeTeamId == team.Id || match.AwayTeamId == team.Id)
            .ToList();

        PlayerStatisticViewModel statistics = tournamentStatistics.FirstOrDefault(statistic => statistic.PlayerId == id)
            ?? new PlayerStatisticViewModel
            {
                TournamentId = team.TournamentId,
                PlayerId = player.Id,
                TeamId = team.Id,
                PlayerName = player.FullName,
                TeamName = team.Name
            };

        AdminPlayerDetailsViewModel viewModel = new()
        {
            Player = player,
            Team = team,
            Tournament = tournament,
            Statistics = statistics,
            TeamPerformance = BuildTeamPerformance(teamMatches, team.Id),
            Age = 20 + (player.Id % 15),
            Status = "Active",
            RecentMatches = BuildAdminPlayerRecentMatches(matchPerformances, tournamentMatches, team.Id),
            PerformanceCards = BuildAdminPlayerPerformanceCards(statistics, matchPerformances, tournamentStatistics)
        };

        return View(viewModel);
    }

    [Route("Admin/Teams/Details/{id:int}")]
    public async Task<IActionResult> TeamDetails(int id)
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

        IReadOnlyList<PlayerSummaryViewModel> players = await playerRepository.GetByTeamIdAsync(id);
        IReadOnlyList<PlayerStatisticViewModel> tournamentStatistics = await playerRepository.GetStatisticsByTournamentIdAsync(team.TournamentId);
        IReadOnlyList<MatchSummaryViewModel> tournamentMatches = await matchRepository.GetByTournamentIdAsync(team.TournamentId);
        IReadOnlyList<MatchSummaryViewModel> teamMatches = tournamentMatches
            .Where(match => match.HomeTeamId == id || match.AwayTeamId == id)
            .OrderBy(match => match.ScheduledAt)
            .ToList();

        TeamPerformanceViewModel performance = BuildTeamPerformance(teamMatches, id);

        AdminTeamDetailsViewModel viewModel = new()
        {
            Team = team,
            Tournament = tournament,
            Performance = performance,
            Players = BuildAdminTeamPlayerRows(players, tournamentStatistics),
            Matches = BuildAdminTeamMatchRows(teamMatches, id),
            StatisticCards = BuildAdminTeamStatisticCards(tournamentStatistics.Where(statistic => statistic.TeamId == id).ToList())
        };

        return View(viewModel);
    }

    [Route("Admin/Tournaments/Details/{id:int}")]
    public async Task<IActionResult> TournamentDetails(int id)
    {
        TournamentCardViewModel? tournament = await tournamentRepository.GetByIdAsync(id);

        if (tournament is null)
        {
            return NotFound();
        }

        IReadOnlyList<TeamSummaryViewModel> teams = await teamRepository.GetByTournamentIdAsync(id);
        IReadOnlyList<MatchSummaryViewModel> matches = await matchRepository.GetByTournamentIdAsync(id);
        IReadOnlyList<GroupStandingViewModel> standings = await tournamentRepository.GetGroupStandingsAsync(id);
        IReadOnlyList<PlayerStatisticViewModel> playerStatistics = await playerRepository.GetStatisticsByTournamentIdAsync(id);
        TournamentStatisticViewModel statistics = await tournamentRepository.GetStatisticsAsync(id) ?? new TournamentStatisticViewModel { TournamentId = id };

        AdminTournamentDetailsViewModel viewModel = new()
        {
            Details = new TournamentDetailsViewModel
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
                    .OrderBy(match => match.ScheduledAt)
                    .ToList(),
                PlayerLeaderboards = BuildPlayerLeaderboards(playerStatistics)
            }
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

    private static AdminTeamRowViewModel BuildAdminTeamRow(
        TeamSummaryViewModel team,
        IReadOnlyList<TournamentCardViewModel> tournaments,
        IReadOnlyList<PlayerSummaryViewModel> players,
        IReadOnlyList<MatchSummaryViewModel> matches)
    {
        IReadOnlyList<MatchSummaryViewModel> completedMatches = matches
            .Where(match => match.Status == "Completed" && (match.HomeTeamId == team.Id || match.AwayTeamId == team.Id))
            .ToList();

        int wins = completedMatches.Count(match => IsWinner(match, team.Id));
        int draws = completedMatches.Count(match => match.HomeScore == match.AwayScore);
        int losses = completedMatches.Count - wins - draws;
        int goalsScored = completedMatches.Sum(match => match.HomeTeamId == team.Id ? match.HomeScore ?? 0 : match.AwayScore ?? 0);
        int goalsConceded = completedMatches.Sum(match => match.HomeTeamId == team.Id ? match.AwayScore ?? 0 : match.HomeScore ?? 0);
        string tournamentName = tournaments.FirstOrDefault(tournament => tournament.Id == team.TournamentId)?.Name ?? string.Empty;

        return new AdminTeamRowViewModel
        {
            Id = team.Id,
            Name = team.Name,
            City = team.City,
            CoachName = team.CoachName,
            PlayerCount = players.Count(player => player.TeamId == team.Id),
            TournamentName = tournamentName,
            Record = $"{wins}-{draws}-{losses}",
            Goals = $"{goalsScored}:{goalsConceded}"
        };
    }

    private async Task<IReadOnlyList<PlayerStatisticViewModel>> BuildAllPlayerStatisticsAsync(IReadOnlyList<TeamSummaryViewModel> teams)
    {
        List<PlayerStatisticViewModel> statistics = [];

        foreach (int tournamentId in teams.Select(team => team.TournamentId).Distinct())
        {
            IReadOnlyList<PlayerStatisticViewModel> tournamentStatistics = await playerRepository.GetStatisticsByTournamentIdAsync(tournamentId);
            statistics.AddRange(tournamentStatistics);
        }

        return statistics;
    }

    private static AdminPlayerRowViewModel BuildAdminPlayerRow(
        PlayerSummaryViewModel player,
        IReadOnlyList<TeamSummaryViewModel> teams,
        IReadOnlyList<PlayerStatisticViewModel> playerStatistics)
    {
        TeamSummaryViewModel? team = teams.FirstOrDefault(item => item.Id == player.TeamId);
        PlayerStatisticViewModel? statistic = playerStatistics.FirstOrDefault(item => item.PlayerId == player.Id);

        return new AdminPlayerRowViewModel
        {
            Id = player.Id,
            FullName = player.FullName,
            TeamName = team?.Name ?? string.Empty,
            Position = player.Position,
            ShirtNumber = player.ShirtNumber,
            Goals = statistic?.Goals ?? 0,
            YellowCards = statistic?.YellowCards ?? 0,
            RedCards = statistic?.RedCards ?? 0,
            TwoMinuteSuspensions = statistic?.TwoMinuteSuspensions ?? 0
        };
    }

    private static AdminMatchRowViewModel BuildAdminMatchRow(
        MatchSummaryViewModel match,
        IReadOnlyList<TournamentCardViewModel> tournaments)
    {
        string tournamentName = tournaments.FirstOrDefault(tournament => tournament.Id == match.TournamentId)?.Name ?? "Unknown Tournament";

        return new AdminMatchRowViewModel
        {
            Id = match.Id,
            HomeTeamName = match.HomeTeamName,
            AwayTeamName = match.AwayTeamName,
            TournamentName = tournamentName,
            Phase = match.RoundName,
            VenueName = match.VenueName,
            RefereeName = string.IsNullOrWhiteSpace(match.RefereeName) ? "Unassigned" : match.RefereeName,
            ScheduledAt = match.ScheduledAt,
            Status = match.Status,
            Score = match.HomeScore.HasValue && match.AwayScore.HasValue
                ? $"{match.HomeScore}:{match.AwayScore}"
                : "Not played"
        };
    }

    private static MatchTotalsViewModel BuildMatchTotals(
        MatchSummaryViewModel match,
        IReadOnlyList<MatchEventSummaryViewModel> events)
    {
        return new MatchTotalsViewModel
        {
            TotalGoals = (match.HomeScore ?? 0) + (match.AwayScore ?? 0),
            YellowCards = events.Count(matchEvent => matchEvent.EventType == "Yellow Card"),
            RedCards = events.Count(matchEvent => matchEvent.EventType == "Red Card"),
            TwoMinuteSuspensions = events.Count(matchEvent => matchEvent.EventType == "Two-Minute Suspension")
        };
    }

    private static TeamMatchComparisonViewModel BuildMatchComparison(
        int teamId,
        string teamName,
        MatchSummaryViewModel match,
        IReadOnlyList<MatchEventSummaryViewModel> events)
    {
        return new TeamMatchComparisonViewModel
        {
            TeamId = teamId,
            TeamName = teamName,
            Goals = match.HomeTeamId == teamId ? match.HomeScore ?? 0 : match.AwayScore ?? 0,
            YellowCards = events.Count(matchEvent => matchEvent.TeamId == teamId && matchEvent.EventType == "Yellow Card"),
            RedCards = events.Count(matchEvent => matchEvent.TeamId == teamId && matchEvent.EventType == "Red Card"),
            TwoMinuteSuspensions = events.Count(matchEvent => matchEvent.TeamId == teamId && matchEvent.EventType == "Two-Minute Suspension")
        };
    }

    private static IReadOnlyList<AdminPlayerRecentMatchViewModel> BuildAdminPlayerRecentMatches(
        IReadOnlyList<PlayerMatchPerformanceViewModel> performances,
        IReadOnlyList<MatchSummaryViewModel> matches,
        int teamId)
    {
        return performances
            .OrderByDescending(performance => performance.MatchDate)
            .Select(performance =>
            {
                MatchSummaryViewModel? match = matches.FirstOrDefault(item => item.Id == performance.MatchId);

                return new AdminPlayerRecentMatchViewModel
                {
                    MatchId = performance.MatchId,
                    MatchDate = performance.MatchDate,
                    OpponentName = performance.OpponentName,
                    Phase = match?.RoundName ?? string.Empty,
                    Result = match is null ? "Not played" : BuildMatchResult(match, teamId),
                    Goals = performance.Goals,
                    YellowCards = performance.YellowCards,
                    RedCards = performance.RedCards,
                    TwoMinuteSuspensions = performance.TwoMinuteSuspensions
                };
            })
            .ToList();
    }

    private static IReadOnlyList<AdminPlayerPerformanceCardViewModel> BuildAdminPlayerPerformanceCards(
        PlayerStatisticViewModel statistic,
        IReadOnlyList<PlayerMatchPerformanceViewModel> performances,
        IReadOnlyList<PlayerStatisticViewModel> tournamentStatistics)
    {
        PlayerMatchPerformanceViewModel? bestMatch = performances
            .OrderByDescending(performance => performance.Goals)
            .ThenBy(performance => performance.OpponentName)
            .FirstOrDefault();

        int goalRank = tournamentStatistics
            .OrderByDescending(item => item.Goals)
            .ThenBy(item => item.PlayerName)
            .Select((item, index) => new { item.PlayerId, Rank = index + 1 })
            .FirstOrDefault(item => item.PlayerId == statistic.PlayerId)?.Rank ?? 0;

        return
        [
            new AdminPlayerPerformanceCardViewModel
            {
                Label = "Best Match",
                Value = bestMatch is null ? "No data" : bestMatch.OpponentName,
                Detail = bestMatch is null ? "No completed matches" : $"{bestMatch.Goals} goals"
            },
            new AdminPlayerPerformanceCardViewModel
            {
                Label = "Highest Goals in One Match",
                Value = bestMatch?.Goals.ToString() ?? "0",
                Detail = bestMatch is null ? "No completed matches" : $"Against {bestMatch.OpponentName}"
            },
            new AdminPlayerPerformanceCardViewModel
            {
                Label = "Current Goal Ranking",
                Value = goalRank == 0 ? "Unranked" : $"#{goalRank}",
                Detail = $"{statistic.Goals} tournament goals"
            },
            new AdminPlayerPerformanceCardViewModel
            {
                Label = "Fair Play Rating",
                Value = statistic.RedCards == 0 && statistic.TwoMinuteSuspensions <= 1 ? "Good" : "Watch",
                Detail = "Placeholder rating"
            }
        ];
    }

    private static TeamPerformanceViewModel BuildTeamPerformance(IReadOnlyList<MatchSummaryViewModel> matches, int teamId)
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

    private static IReadOnlyList<AdminTeamPlayerRowViewModel> BuildAdminTeamPlayerRows(
        IReadOnlyList<PlayerSummaryViewModel> players,
        IReadOnlyList<PlayerStatisticViewModel> tournamentStatistics)
    {
        return players
            .OrderBy(player => player.ShirtNumber)
            .Select(player =>
            {
                PlayerStatisticViewModel? statistic = tournamentStatistics.FirstOrDefault(item => item.PlayerId == player.Id);

                return new AdminTeamPlayerRowViewModel
                {
                    Id = player.Id,
                    ShirtNumber = player.ShirtNumber,
                    FullName = player.FullName,
                    Position = player.Position,
                    Age = 20 + (player.Id % 15),
                    Goals = statistic?.Goals ?? 0,
                    YellowCards = statistic?.YellowCards ?? 0,
                    RedCards = statistic?.RedCards ?? 0,
                    TwoMinuteSuspensions = statistic?.TwoMinuteSuspensions ?? 0
                };
            })
            .ToList();
    }

    private static IReadOnlyList<AdminTeamMatchRowViewModel> BuildAdminTeamMatchRows(
        IReadOnlyList<MatchSummaryViewModel> matches,
        int teamId)
    {
        return matches
            .Select(match => new AdminTeamMatchRowViewModel
            {
                Id = match.Id,
                OpponentName = match.HomeTeamId == teamId ? match.AwayTeamName : match.HomeTeamName,
                Phase = match.RoundName,
                ScheduledAt = match.ScheduledAt,
                Result = BuildMatchResult(match, teamId),
                Status = match.Status
            })
            .ToList();
    }

    private static string BuildMatchResult(MatchSummaryViewModel match, int teamId)
    {
        if (!match.HomeScore.HasValue || !match.AwayScore.HasValue)
        {
            return "Not played";
        }

        int teamScore = match.HomeTeamId == teamId ? match.HomeScore.Value : match.AwayScore.Value;
        int opponentScore = match.HomeTeamId == teamId ? match.AwayScore.Value : match.HomeScore.Value;

        return $"{teamScore}:{opponentScore}";
    }

    private static IReadOnlyList<AdminTeamStatisticCardViewModel> BuildAdminTeamStatisticCards(
        IReadOnlyList<PlayerStatisticViewModel> statistics)
    {
        return
        [
            BuildAdminTeamStatisticCard("Top Scorer", "goals", statistics.OrderByDescending(statistic => statistic.Goals), statistic => statistic.Goals),
            BuildAdminTeamStatisticCard("Most Yellow Cards", "yellow cards", statistics.OrderByDescending(statistic => statistic.YellowCards), statistic => statistic.YellowCards),
            BuildAdminTeamStatisticCard("Most Red Cards", "red cards", statistics.OrderByDescending(statistic => statistic.RedCards), statistic => statistic.RedCards),
            BuildAdminTeamStatisticCard("Most Two-Minute Suspensions", "suspensions", statistics.OrderByDescending(statistic => statistic.TwoMinuteSuspensions), statistic => statistic.TwoMinuteSuspensions)
        ];
    }

    private static AdminTeamStatisticCardViewModel BuildAdminTeamStatisticCard(
        string title,
        string valueName,
        IOrderedEnumerable<PlayerStatisticViewModel> orderedStatistics,
        Func<PlayerStatisticViewModel, int> valueSelector)
    {
        PlayerStatisticViewModel? leader = orderedStatistics
            .ThenBy(statistic => statistic.PlayerName)
            .FirstOrDefault();

        if (leader is null)
        {
            return new AdminTeamStatisticCardViewModel
            {
                Title = title,
                PlayerName = "No data",
                ValueLabel = $"0 {valueName}"
            };
        }

        return new AdminTeamStatisticCardViewModel
        {
            Title = title,
            PlayerId = leader.PlayerId,
            PlayerName = leader.PlayerName,
            ValueLabel = $"{valueSelector(leader)} {valueName}"
        };
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
                    PlayerId = statistic.PlayerId,
                    PlayerName = statistic.PlayerName,
                    TeamName = statistic.TeamName,
                    Value = valueSelector(statistic)
                })
                .ToList()
        };
    }
}

using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class MatchesController : Controller
    {
        private readonly MatchRepository _matchRepository;
        private readonly TeamRepository _teamRepository;
        private readonly GroupPhaseRepository _groupPhaseRepository;
        private readonly CompetitionRepository _competitionRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<MatchesController> _logger;
        private static readonly HashSet<string> AllowedReportExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".doc",
            ".docx",
            ".jpg",
            ".jpeg",
            ".png"
        };

        public MatchesController(
            MatchRepository matchRepository,
            TeamRepository teamRepository,
            GroupPhaseRepository groupPhaseRepository,
            CompetitionRepository competitionRepository,
            IWebHostEnvironment environment,
            UserManager<AppUser> userManager,
            ILogger<MatchesController> logger)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _groupPhaseRepository = groupPhaseRepository;
            _competitionRepository = competitionRepository;
            _environment = environment;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(string? query)
        {
            _logger.LogInformation("Matches Index page requested");
            ViewData["Query"] = query;
            var matches = GetAccessibleMatches(query)
                .OrderBy(m => m.Kickoff)
                .ToList();
            
            var teamNames = new Dictionary<int, string>();
            foreach (var match in matches)
            {
                if (!teamNames.ContainsKey(match.HomeTeamId))
                    teamNames[match.HomeTeamId] = _teamRepository.GetById(match.HomeTeamId)?.Name ?? $"Team {match.HomeTeamId}";
                if (!teamNames.ContainsKey(match.AwayTeamId))
                    teamNames[match.AwayTeamId] = _teamRepository.GetById(match.AwayTeamId)?.Name ?? $"Team {match.AwayTeamId}";
            }
            
            ViewBag.TeamNames = teamNames;
            return View(matches);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Autocomplete(string? query)
        {
            var matches = GetAccessibleMatches(query);

            var suggestions = matches
                .OrderBy(m => m.Kickoff)
                .Take(8)
                .Select(m => new
                {
                    label = $"{m.HomeTeam?.Name ?? $"Team {m.HomeTeamId}"} vs {m.AwayTeam?.Name ?? $"Team {m.AwayTeamId}"}",
                    value = m.HomeTeam?.Name ?? m.AwayTeam?.Name ?? m.MaintenanceHall,
                    meta = $"{m.Status} - {m.MaintenanceHall}"
                });

            return Json(suggestions);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Matches Details page requested for match ID: {MatchId}", id);
            var match = GetAccessibleMatch(id);
            
            if (match == null)
            {
                _logger.LogWarning("Match with ID {MatchId} not found", id);
                return NotFound();
            }

            var homeTeamName = _teamRepository.GetById(match.HomeTeamId)?.Name ?? $"Team {match.HomeTeamId}";
            var awayTeamName = _teamRepository.GetById(match.AwayTeamId)?.Name ?? $"Team {match.AwayTeamId}";
            var competitionName = _competitionRepository.GetById(match.CompetitionId)?.Name ?? $"Competition {match.CompetitionId}";
            
            ViewBag.HomeTeamName = homeTeamName;
            ViewBag.AwayTeamName = awayTeamName;
            ViewBag.CompetitionName = competitionName;
            ViewBag.TeamRepository = _teamRepository;
            
            return View(match);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateResult(int id, int homeScore, int awayScore, MatchStatus status)
        {
            var match = GetAccessibleMatch(id);

            if (match == null)
            {
                return NotFound();
            }

            if (homeScore < 0 || awayScore < 0)
            {
                TempData["ErrorNotification"] = "Match result cannot contain negative values.";
                return RedirectToAction(nameof(Details), new { id });
            }

            match.HomeScore = homeScore;
            match.AwayScore = awayScore;
            match.Status = status;
            _matchRepository.Update(match);

            TempData["SuccessNotification"] = "Match result updated successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadReport(int id, IFormFile? reportFile)
        {
            var match = GetAccessibleMatch(id);

            if (match == null)
            {
                return NotFound();
            }

            if (reportFile == null || reportFile.Length == 0)
            {
                TempData["ErrorNotification"] = "Select a match report file before uploading.";
                return RedirectToAction(nameof(Details), new { id });
            }

            if (reportFile.Length > 5 * 1024 * 1024)
            {
                TempData["ErrorNotification"] = "Match report file cannot be larger than 5 MB.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var extension = Path.GetExtension(reportFile.FileName);

            if (!AllowedReportExtensions.Contains(extension))
            {
                TempData["ErrorNotification"] = "Allowed report formats are PDF, DOC, DOCX, JPG, JPEG, and PNG.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var uploadsDirectory = Path.Combine(_environment.WebRootPath, "uploads", "match-reports");
            Directory.CreateDirectory(uploadsDirectory);

            var storedFileName = $"match-{match.Id}-{Guid.NewGuid():N}{extension}";
            var storedPath = Path.Combine(uploadsDirectory, storedFileName);

            await using (var stream = System.IO.File.Create(storedPath))
            {
                await reportFile.CopyToAsync(stream);
            }

            match.ReportFileName = Path.GetFileName(reportFile.FileName);
            match.ReportFilePath = $"/uploads/match-reports/{storedFileName}";
            _matchRepository.Update(match);

            TempData["SuccessNotification"] = "Match report uploaded successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveReport(int id)
        {
            var match = GetAccessibleMatch(id);

            if (match == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(match.ReportFilePath))
            {
                var relativePath = match.ReportFilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                var physicalPath = Path.Combine(_environment.WebRootPath, relativePath);

                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }

            match.ReportFileName = null;
            match.ReportFilePath = null;
            _matchRepository.Update(match);

            TempData["SuccessNotification"] = "Match report removed successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        [Authorize]
        [Route("matches/status/{status}")]
        public IActionResult GetByStatus(string status)
        {
            _logger.LogInformation("Matches by status requested: {Status}", status);
            
            if (!Enum.TryParse<MatchStatus>(status, true, out var matchStatus))
            {
                _logger.LogWarning("Invalid match status: {Status}", status);
                return BadRequest("Invalid match status. Valid values are: Scheduled, Live, Finished, Cancelled");
            }

            var matches = (IsAdmin()
                    ? _matchRepository.GetByStatus(matchStatus)
                    : IsCoach()
                        ? _matchRepository.GetByStatusForCoach(matchStatus, CurrentUserDisplayName)
                        : _matchRepository.GetByStatusForUser(matchStatus, CurrentUserId))
                .OrderBy(m => m.Kickoff)
                .ToList();
            
            var teamNames = new Dictionary<int, string>();
            foreach (var match in matches)
            {
                if (!teamNames.ContainsKey(match.HomeTeamId))
                    teamNames[match.HomeTeamId] = _teamRepository.GetById(match.HomeTeamId)?.Name ?? $"Team {match.HomeTeamId}";
                if (!teamNames.ContainsKey(match.AwayTeamId))
                    teamNames[match.AwayTeamId] = _teamRepository.GetById(match.AwayTeamId)?.Name ?? $"Team {match.AwayTeamId}";
            }
            
            ViewBag.TeamNames = teamNames;
            return View("Index", matches);
        }

        private bool IsAdmin() => User.IsInRole("Admin");

        private bool IsCoach() => User.IsInRole("Coach");

        private List<Match> GetAccessibleMatches(string? query)
        {
            if (IsAdmin())
            {
                return _matchRepository.Search(query);
            }

            if (IsCoach())
            {
                return _matchRepository.SearchForCoach(query, CurrentUserDisplayName);
            }

            return _matchRepository.SearchForUser(query, CurrentUserId);
        }

        private Match? GetAccessibleMatch(int id)
        {
            if (IsAdmin())
            {
                return _matchRepository.GetById(id);
            }

            if (IsCoach())
            {
                return _matchRepository.GetByIdForCoach(id, CurrentUserDisplayName);
            }

            return _matchRepository.GetByIdForUser(id, CurrentUserId);
        }

        private int CurrentUserId
        {
            get
            {
                var userId = _userManager.GetUserId(User);
                return int.TryParse(userId, out var parsedUserId) ? parsedUserId : 0;
            }
        }

        private string CurrentUserDisplayName =>
            _userManager.GetUserAsync(User).GetAwaiter().GetResult()?.DisplayName ?? string.Empty;
    }
}

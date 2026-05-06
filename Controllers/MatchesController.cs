using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class MatchesController : Controller
    {
        private readonly MatchRepository _matchRepository;
        private readonly TeamRepository _teamRepository;
        private readonly GroupPhaseRepository _groupPhaseRepository;
        private readonly ILogger<MatchesController> _logger;

        public MatchesController(
            MatchRepository matchRepository,
            TeamRepository teamRepository,
            GroupPhaseRepository groupPhaseRepository,
            ILogger<MatchesController> logger)
        {
            _matchRepository = matchRepository;
            _teamRepository = teamRepository;
            _groupPhaseRepository = groupPhaseRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Matches Index page requested");
            var matches = _matchRepository.GetAll().OrderBy(m => m.Kickoff).ToList();
            
            // Build a dictionary of team names for view
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
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Matches Details page requested for match ID: {MatchId}", id);
            var match = _matchRepository.GetById(id);
            
            if (match == null)
            {
                _logger.LogWarning("Match with ID {MatchId} not found", id);
                return NotFound();
            }

            var homeTeamName = _teamRepository.GetById(match.HomeTeamId)?.Name ?? $"Team {match.HomeTeamId}";
            var awayTeamName = _teamRepository.GetById(match.AwayTeamId)?.Name ?? $"Team {match.AwayTeamId}";
            
            ViewBag.HomeTeamName = homeTeamName;
            ViewBag.AwayTeamName = awayTeamName;
            ViewBag.TeamRepository = _teamRepository;
            
            return View(match);
        }

        [HttpGet]
        [Route("matches/status/{status}")]
        public IActionResult GetByStatus(string status)
        {
            _logger.LogInformation("Matches by status requested: {Status}", status);
            
            if (!Enum.TryParse<MatchStatus>(status, true, out var matchStatus))
            {
                _logger.LogWarning("Invalid match status: {Status}", status);
                return BadRequest("Invalid match status. Valid values are: Scheduled, Live, Finished, Cancelled");
            }

            var matches = _matchRepository.GetByStatus(matchStatus).OrderBy(m => m.Kickoff).ToList();
            
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
    }
}

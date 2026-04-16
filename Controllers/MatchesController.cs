using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories.Mock;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class MatchesController : Controller
    {
        private readonly MatchMockRepository _matchRepository;
        private readonly TeamMockRepository _teamRepository;
        private readonly GroupPhaseMockRepository _groupPhaseRepository;
        private readonly ILogger<MatchesController> _logger;

        public MatchesController(
            MatchMockRepository matchRepository,
            TeamMockRepository teamRepository,
            GroupPhaseMockRepository groupPhaseRepository,
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
    }
}

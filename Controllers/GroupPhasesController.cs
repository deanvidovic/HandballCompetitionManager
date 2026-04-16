using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories.Mock;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class GroupPhasesController : Controller
    {
        private readonly GroupPhaseMockRepository _groupPhaseRepository;
        private readonly CompetitionMockRepository _competitionRepository;
        private readonly TeamMockRepository _teamRepository;
        private readonly ILogger<GroupPhasesController> _logger;

        public GroupPhasesController(GroupPhaseMockRepository groupPhaseRepository, CompetitionMockRepository competitionRepository, TeamMockRepository teamRepository, ILogger<GroupPhasesController> logger)
        {
            _groupPhaseRepository = groupPhaseRepository;
            _competitionRepository = competitionRepository;
            _teamRepository = teamRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("GroupPhases Index page requested");
            var groupPhases = _groupPhaseRepository.GetAll();
            return View(groupPhases);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("GroupPhases Details page requested for group phase ID: {GroupPhaseId}", id);
            var groupPhase = _groupPhaseRepository.GetById(id);
            
            if (groupPhase == null)
            {
                _logger.LogWarning("GroupPhase with ID {GroupPhaseId} not found", id);
                return NotFound();
            }

            ViewBag.TeamRepository = _teamRepository;
            return View(groupPhase);
        }
    }
}

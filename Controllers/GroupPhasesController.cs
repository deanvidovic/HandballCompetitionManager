using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class GroupPhasesController : Controller
    {
        private readonly GroupPhaseRepository _groupPhaseRepository;
        private readonly CompetitionRepository _competitionRepository;
        private readonly TeamRepository _teamRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<GroupPhasesController> _logger;

        public GroupPhasesController(GroupPhaseRepository groupPhaseRepository, CompetitionRepository competitionRepository, TeamRepository teamRepository, UserManager<AppUser> userManager, ILogger<GroupPhasesController> logger)
        {
            _groupPhaseRepository = groupPhaseRepository;
            _competitionRepository = competitionRepository;
            _teamRepository = teamRepository;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            _logger.LogInformation("GroupPhases Index page requested");
            var groupPhases = IsAdmin()
                ? _groupPhaseRepository.GetAll()
                : _groupPhaseRepository.GetAllForUser(CurrentUserId);
            return View(groupPhases);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("GroupPhases Details page requested for group phase ID: {GroupPhaseId}", id);
            var groupPhase = IsAdmin()
                ? _groupPhaseRepository.GetById(id)
                : _groupPhaseRepository.GetByIdForUser(id, CurrentUserId);
            
            if (groupPhase == null)
            {
                _logger.LogWarning("GroupPhase with ID {GroupPhaseId} not found", id);
                return NotFound();
            }

            ViewBag.TeamRepository = _teamRepository;
            return View(groupPhase);
        }

        private bool IsAdmin() => User.IsInRole("Admin");

        private int CurrentUserId
        {
            get
            {
                var userId = _userManager.GetUserId(User);
                return int.TryParse(userId, out var parsedUserId) ? parsedUserId : 0;
            }
        }
    }
}

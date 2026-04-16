using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories.Mock;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class TeamsController : Controller
    {
        private readonly TeamMockRepository _teamRepository;
        private readonly PlayerMockRepository _playerRepository;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(TeamMockRepository teamRepository, PlayerMockRepository playerRepository, ILogger<TeamsController> logger)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Teams Index page requested");
            var teams = _teamRepository.GetAll();
            return View(teams);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Teams Details page requested for team ID: {TeamId}", id);
            var team = _teamRepository.GetById(id);
            
            if (team == null)
            {
                _logger.LogWarning("Team with ID {TeamId} not found", id);
                return NotFound();
            }

            return View(team);
        }
    }
}

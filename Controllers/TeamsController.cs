using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class TeamsController : Controller
    {
        private readonly TeamRepository _teamRepository;
        private readonly PlayerRepository _playerRepository;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(TeamRepository teamRepository, PlayerRepository playerRepository, ILogger<TeamsController> logger)
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

        [HttpGet]
        [Route("teams/city/{city}")]
        public IActionResult GetByCity(string city)
        {
            _logger.LogInformation("Teams by city requested: {City}", city);
            var teams = _teamRepository.GetByCity(city);
            return View("Index", teams);
        }
    }
}

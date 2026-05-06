using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class PlayersController : Controller
    {
        private readonly PlayerRepository _playerRepository;
        private readonly TeamRepository _teamRepository;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(PlayerRepository playerRepository, TeamRepository teamRepository, ILogger<PlayersController> logger)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Players Index page requested");
            var players = _playerRepository.GetAll();
            return View(players);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Players Details page requested for player ID: {PlayerId}", id);
            var player = _playerRepository.GetById(id);
            
            if (player == null)
            {
                _logger.LogWarning("Player with ID {PlayerId} not found", id);
                return NotFound();
            }

            return View(player);
        }

        [HttpGet]
        [Route("teams/{teamId}/players")]
        public IActionResult GetByTeam(int teamId)
        {
            _logger.LogInformation("Players by team requested: {TeamId}", teamId);
            var players = _playerRepository.GetByTeamId(teamId);
            return View("Index", players);
        }
    }
}

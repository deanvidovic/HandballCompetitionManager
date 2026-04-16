using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories.Mock;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class PlayersController : Controller
    {
        private readonly PlayerMockRepository _playerRepository;
        private readonly TeamMockRepository _teamRepository;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(PlayerMockRepository playerRepository, TeamMockRepository teamRepository, ILogger<PlayersController> logger)
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
    }
}

using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories.Mock;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class CompetitionsController : Controller
    {
        private readonly CompetitionMockRepository _competitionRepository;
        private readonly ILogger<CompetitionsController> _logger;

        public CompetitionsController(CompetitionMockRepository competitionRepository, ILogger<CompetitionsController> logger)
        {
            _competitionRepository = competitionRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Competitions Index page requested");
            var competitions = _competitionRepository.GetAll();
            return View(competitions);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("Competitions Details page requested for competition ID: {CompetitionId}", id);
            var competition = _competitionRepository.GetById(id);
            
            if (competition == null)
            {
                _logger.LogWarning("Competition with ID {CompetitionId} not found", id);
                return NotFound();
            }

            return View(competition);
        }
    }
}

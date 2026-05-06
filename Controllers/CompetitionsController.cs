using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class CompetitionsController : Controller
    {
        private readonly CompetitionRepository _competitionRepository;
        private readonly ILogger<CompetitionsController> _logger;

        public CompetitionsController(CompetitionRepository competitionRepository, ILogger<CompetitionsController> logger)
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

        [HttpGet]
        [Route("competitions/city/{city}")]
        public IActionResult GetByCity(string city)
        {
            _logger.LogInformation("Competitions by city requested: {City}", city);
            var competitions = _competitionRepository.GetByCity(city);
            return View("Index", competitions);
        }
    }
}

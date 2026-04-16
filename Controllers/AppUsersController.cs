using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories.Mock;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly AppUserMockRepository _appUserRepository;
        private readonly ILogger<AppUsersController> _logger;

        public AppUsersController(AppUserMockRepository appUserRepository, ILogger<AppUsersController> logger)
        {
            _appUserRepository = appUserRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("AppUsers Index page requested");
            var users = _appUserRepository.GetAll();
            return View(users);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            _logger.LogInformation("AppUsers Details page requested for user ID: {UserId}", id);
            var user = _appUserRepository.GetById(id);
            
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound();
            }

            return View(user);
        }
    }
}

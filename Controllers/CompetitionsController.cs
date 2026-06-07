using HandballCompetitionManager.Models;
using HandballCompetitionManager.Repositories;
using HandballCompetitionManager.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandballCompetitionManager.Controllers
{
    public class CompetitionsController : Controller
    {
        private readonly CompetitionRepository _competitionRepository;
        private readonly GroupPhaseRepository _groupPhaseRepository;
        private readonly TeamRepository _teamRepository;
        private readonly ILogger<CompetitionsController> _logger;

        public CompetitionsController(CompetitionRepository competitionRepository, GroupPhaseRepository groupPhaseRepository, TeamRepository teamRepository, ILogger<CompetitionsController> logger)
        {
            _competitionRepository = competitionRepository;
            _groupPhaseRepository = groupPhaseRepository;
            _teamRepository = teamRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string? query)
        {
            _logger.LogInformation("Competitions Index page requested");
            ViewData["Query"] = query;
            var competitions = _competitionRepository.Search(query);
            return View(competitions);
        }

        [HttpGet]
        public IActionResult Autocomplete(string? query)
        {
            var suggestions = _competitionRepository.Search(query)
                .Take(8)
                .Select(c => new
                {
                    label = c.Name,
                    value = c.Name,
                    meta = $"{c.City} - {c.Season}"
                });

            return Json(suggestions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCompetitionViewModel model)
        {
            ValidateCompetitionSeason(model);

            if (model.StartDate.HasValue && model.EndDate.HasValue && model.StartDate.Value.Date > model.EndDate.Value.Date)
            {
                ModelState.AddModelError(nameof(model.StartDate), "Start date cannot be after end date.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CreateValidationResponse());
            }

            var competition = new Competition
            {
                Name = model.Name.Trim(),
                Season = model.Season.Trim(),
                StartDate = model.StartDate!.Value.Date,
                EndDate = model.EndDate!.Value.Date,
                City = model.City.Trim()
            };

            _competitionRepository.Add(competition);
            _logger.LogInformation("Competition created: {CompetitionName}", competition.Name);

            return Json(new
            {
                success = true,
                message = "Competition created successfully."
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateCompetitionViewModel model)
        {
            var competition = _competitionRepository.GetById(model.Id);

            if (competition == null)
            {
                return NotFound();
            }

            ValidateCompetitionSeason(model);

            if (model.StartDate.HasValue && model.EndDate.HasValue && model.StartDate.Value.Date > model.EndDate.Value.Date)
            {
                ModelState.AddModelError(nameof(model.StartDate), "Start date cannot be after end date.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(CreateValidationResponse());
            }

            competition.Name = model.Name.Trim();
            competition.Season = model.Season.Trim();
            competition.StartDate = model.StartDate!.Value.Date;
            competition.EndDate = model.EndDate!.Value.Date;
            competition.City = model.City.Trim();

            _competitionRepository.Update(competition);
            _logger.LogInformation("Competition updated: {CompetitionName}", competition.Name);

            return Json(new
            {
                success = true,
                message = "Competition updated successfully."
            });
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

            var viewModel = BuildDetailsViewModel(competition);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AvailableTeamsAutocomplete(int competitionId, string? query)
        {
            var competition = _competitionRepository.GetById(competitionId);

            if (competition == null)
            {
                return NotFound();
            }

            var existingTeamIds = (competition.Teams ?? new List<Team>())
                .Select(team => team.Id)
                .ToHashSet();
            var teams = _teamRepository.GetAvailableForCompetition(competition.StartDate, competition.EndDate, competition.Id)
                .Where(team => !existingTeamIds.Contains(team.Id));

            if (!string.IsNullOrWhiteSpace(query))
            {
                var normalizedQuery = query.Trim().ToLowerInvariant();
                teams = teams.Where(team =>
                    team.Name.ToLower().Contains(normalizedQuery) ||
                    team.HomeCity.ToLower().Contains(normalizedQuery) ||
                    team.CoachName.ToLower().Contains(normalizedQuery));
            }

            var suggestions = teams
                .OrderBy(team => team.Name)
                .Take(8)
                .Select(team => new
                {
                    id = team.Id,
                    label = team.Name,
                    value = team.Name,
                    coach = team.CoachName,
                    city = team.HomeCity,
                    players = team.Players?.Count ?? 0,
                    meta = $"{team.HomeCity} - {team.CoachName}"
                });

            return Json(suggestions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGroup(CompetitionDetailsViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewGroupName))
            {
                ModelState.AddModelError(nameof(model.NewGroupName), "Group name is required.");
            }

            if (!ModelState.IsValid)
            {
                var competition = _competitionRepository.GetById(model.CompetitionId);

                if (competition == null)
                {
                    _logger.LogWarning("Competition with ID {CompetitionId} not found while adding group", model.CompetitionId);
                    return NotFound();
                }

                var viewModel = BuildDetailsViewModel(competition);
                viewModel.NewGroupName = model.NewGroupName ?? string.Empty;
                return View("Details", viewModel);
            }

            var existingCompetition = _competitionRepository.GetById(model.CompetitionId);

            if (existingCompetition == null)
            {
                _logger.LogWarning("Competition with ID {CompetitionId} not found while adding group", model.CompetitionId);
                return NotFound();
            }

            var groupPhase = new GroupPhase
            {
                Name = model.NewGroupName.Trim(),
                CompetitionId = existingCompetition.Id
            };

            _groupPhaseRepository.Add(groupPhase);

            return RedirectToAction("Details", new { id = existingCompetition.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTeam(CompetitionDetailsViewModel model)
        {
            var competition = _competitionRepository.GetById(model.CompetitionId);
            var hasTeamSelectionErrors = false;

            if (competition == null)
            {
                _logger.LogWarning("Competition with ID {CompetitionId} not found while adding team", model.CompetitionId);
                return NotFound();
            }

            var selectedTeamIds = model.AddTeamIds.Distinct().ToList();
            var existingTeamIds = (competition.Teams ?? new List<Team>())
                .Select(team => team.Id)
                .ToHashSet();

            if (selectedTeamIds.Count == 0)
            {
                ModelState.AddModelError(nameof(model.AddTeamIds), "Select at least one available team.");
                hasTeamSelectionErrors = true;
            }

            var availableTeams = _teamRepository.GetAvailableForCompetition(competition.StartDate, competition.EndDate, competition.Id);
            var availableTeamIds = availableTeams
                .Where(team => !existingTeamIds.Contains(team.Id))
                .Select(team => team.Id)
                .ToHashSet();
            var invalidTeamIds = selectedTeamIds.Where(teamId => !availableTeamIds.Contains(teamId)).ToList();

            if (invalidTeamIds.Count > 0)
            {
                ModelState.AddModelError(nameof(model.AddTeamIds), "One or more selected teams are already assigned to a competition in this period.");
                hasTeamSelectionErrors = true;
            }

            if (hasTeamSelectionErrors)
            {
                var viewModel = BuildDetailsViewModel(competition);
                viewModel.AddTeamIds = selectedTeamIds;
                return View("Details", viewModel);
            }

            var addedCount = _competitionRepository.AddTeams(competition.Id, selectedTeamIds);
            _logger.LogInformation("{TeamCount} teams added to competition {CompetitionName}", addedCount, competition.Name);

            TempData["CompetitionNotification"] = addedCount == 1
                ? $"1 team added to {competition.Name}."
                : $"{addedCount} teams added to {competition.Name}.";
            return RedirectToAction("Details", new { id = competition.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveTeam(int competitionId, int teamId)
        {
            var competition = _competitionRepository.GetById(competitionId);

            if (competition == null)
            {
                _logger.LogWarning("Competition with ID {CompetitionId} not found while removing team", competitionId);
                return NotFound();
            }

            var removed = _competitionRepository.RemoveTeam(competitionId, teamId);

            if (removed)
            {
                TempData["CompetitionNotification"] = "Team removed from competition.";
            }
            else
            {
                TempData["CompetitionErrorNotification"] = "Team could not be removed from competition.";
            }

            return RedirectToAction("Details", new { id = competition.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateBracket(int competitionId)
        {
            var competition = _competitionRepository.GetById(competitionId);

            if (competition == null)
            {
                _logger.LogWarning("Competition with ID {CompetitionId} not found while generating bracket", competitionId);
                return NotFound();
            }

            var result = _competitionRepository.GenerateBracket(competitionId);

            if (result.Success)
            {
                TempData["CompetitionNotification"] = result.Message;
            }
            else
            {
                TempData["CompetitionErrorNotification"] = result.Message;
            }

            return RedirectToAction("Details", new { id = competitionId });
        }

        [HttpGet]
        [Route("competitions/city/{city}")]
        public IActionResult GetByCity(string city)
        {
            _logger.LogInformation("Competitions by city requested: {City}", city);
            var competitions = _competitionRepository.GetByCity(city);
            return View("Index", competitions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var competition = _competitionRepository.GetById(id);

            if (competition == null)
            {
                TempData["ErrorNotification"] = "Competition was not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var competitionName = competition.Name;
                _competitionRepository.Delete(id);
                _logger.LogInformation("Competition deleted: {CompetitionName}", competitionName);
                TempData["SuccessNotification"] = $"{competitionName} deleted successfully.";
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Competition with ID {CompetitionId} could not be deleted", id);
                TempData["ErrorNotification"] = "Competition could not be deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        private CompetitionDetailsViewModel BuildDetailsViewModel(Competition competition)
        {
            var groups = _groupPhaseRepository.GetByCompetitionId(competition.Id) ?? new List<GroupPhase>();
            var teams = competition.Teams ?? new List<Team>();
            var availableTeams = _teamRepository.GetAvailableForCompetition(competition.StartDate, competition.EndDate, competition.Id)
                .Where(team => teams.All(existingTeam => existingTeam.Id != team.Id))
                .ToList();
            var teamCount = teams.Count;
            var canGenerateBracket = teamCount is 6 or 8 or 12 or 16;
            var groupCount = teamCount <= 8 ? 2 : 4;
            var groupSize = canGenerateBracket ? teamCount / groupCount : 0;
            var knockoutStart = canGenerateBracket
                ? groupCount == 2 ? "Semi-finals" : "Quarter finals"
                : string.Empty;
            var bracketRuleMessage = canGenerateBracket
                ? $"{groupCount} groups of {groupSize}. Top 2 teams from each group advance to {knockoutStart.ToLower()}."
                : "Bracket generation supports 6, 8, 12, or 16 teams.";

            return new CompetitionDetailsViewModel
            {
                CompetitionId = competition.Id,
                Name = competition.Name,
                Season = competition.Season,
                City = competition.City,
                StartDate = competition.StartDate,
                EndDate = competition.EndDate,
                TeamCount = competition.Teams?.Count ?? 0,
                GroupCount = groups?.Count ?? 0,
                CanGenerateBracket = canGenerateBracket,
                BracketRuleMessage = bracketRuleMessage,
                KnockoutStartLabel = knockoutStart,
                Teams = teams.Select(team => new CompetitionTeamViewModel
                {
                    Id = team.Id,
                    Name = team.Name,
                    CoachName = team.CoachName,
                    HomeCity = team.HomeCity,
                    PlayerCount = team.Players?.Count ?? 0
                }).ToList(),
                AvailableTeams = availableTeams.Select(team => new CompetitionTeamViewModel
                {
                    Id = team.Id,
                    Name = team.Name,
                    CoachName = team.CoachName,
                    HomeCity = team.HomeCity,
                    PlayerCount = team.Players?.Count ?? 0
                }).ToList(),
                Groups = groups.Select(group => new CompetitionGroupViewModel
                {
                    Id = group.Id,
                    Name = group.Name,
                    TeamCount = group.Teams?.Count ?? 0,
                    MatchCount = group.Matches?.Count ?? 0,
                    Teams = group.Teams?.Select(team => new CompetitionTeamViewModel
                    {
                        Id = team.Id,
                        Name = team.Name,
                        CoachName = team.CoachName,
                        HomeCity = team.HomeCity,
                        PlayerCount = team.Players?.Count ?? 0
                    }).ToList() ?? new List<CompetitionTeamViewModel>()
                }).ToList()
            };
        }

        private object CreateValidationResponse()
        {
            var fieldErrors = ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

            return new
            {
                success = false,
                errors = fieldErrors.SelectMany(entry => entry.Value).ToList(),
                fieldErrors
            };
        }

        private void ValidateCompetitionSeason(CreateCompetitionViewModel model)
        {
            var season = model.Season?.Trim() ?? string.Empty;
            var parts = season.Split('/');

            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out var firstYear) ||
                !int.TryParse(parts[1], out var secondYear))
            {
                return;
            }

            if (firstYear > secondYear)
            {
                ModelState.AddModelError(nameof(model.Season), "First season year cannot be greater than second season year.");
            }

            if (secondYear > DateTime.Today.Year)
            {
                ModelState.AddModelError(nameof(model.Season), $"Second season year cannot be greater than {DateTime.Today.Year}.");
            }
        }
    }
}

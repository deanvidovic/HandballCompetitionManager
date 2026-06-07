using System.ComponentModel.DataAnnotations;

namespace HandballCompetitionManager.ViewModels;

public class CompetitionDetailsViewModel
{
    [Display(Name = "Competition Id")]
    public int CompetitionId { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "Competition Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "Season")]
    public string Season { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Teams")]
    public int TeamCount { get; set; }

    [Display(Name = "Groups")]
    public int GroupCount { get; set; }

    [Display(Name = "Competition Bracket")]
    public string BracketSectionTitle { get; set; } = "Competition Bracket";

    public bool CanGenerateBracket { get; set; }

    public string BracketRuleMessage { get; set; } = string.Empty;

    public string KnockoutStartLabel { get; set; } = string.Empty;

    [Display(Name = "Teams")]
    public List<CompetitionTeamViewModel> Teams { get; set; } = new();

    [Display(Name = "Available Teams")]
    public List<CompetitionTeamViewModel> AvailableTeams { get; set; } = new();

    [Display(Name = "Teams")]
    public List<int> AddTeamIds { get; set; } = new();

    [Display(Name = "Competition Bracket Groups")]
    public List<CompetitionGroupViewModel> Groups { get; set; } = new();

    [Required]
    [StringLength(80, MinimumLength = 2)]
    [Display(Name = "Group Name")]
    public string NewGroupName { get; set; } = string.Empty;

    [Display(Name = "Competition Bracket")]
    public string GroupsSectionTitle { get; set; } = "Competition Bracket";

    [Display(Name = "Add Group")]
    public string AddGroupTitle { get; set; } = "Add Group";

    [Display(Name = "Create Group")]
    public string AddGroupButtonText { get; set; } = "Create Group";

    [Display(Name = "No bracket has been generated for this competition yet.")]
    public string NoGroupsMessage { get; set; } = "No competition bracket has been generated yet.";
}

public class CompetitionTeamViewModel
{
    [Display(Name = "Team Id")]
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "Team Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(120)]
    [Display(Name = "Coach")]
    public string CoachName { get; set; } = string.Empty;

    [StringLength(120)]
    [Display(Name = "Home City")]
    public string HomeCity { get; set; } = string.Empty;

    [Display(Name = "Players")]
    public int PlayerCount { get; set; }
}

public class CompetitionGroupViewModel
{
    [Display(Name = "Group Id")]
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "Group Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Teams")]
    public int TeamCount { get; set; }

    [Display(Name = "Matches")]
    public int MatchCount { get; set; }

    [Display(Name = "Teams")]
    public List<CompetitionTeamViewModel> Teams { get; set; } = new();
}

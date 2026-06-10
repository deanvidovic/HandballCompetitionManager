using System.ComponentModel.DataAnnotations;
using HandballCompetitionManager.Models;

namespace HandballCompetitionManager.Dtos;

public record AppUserDto(
    int Id,
    string Username,
    string DisplayName,
    string Email,
    UserRole Role,
    DateTime? DateOfBirth,
    DateTime CreatedAt,
    IReadOnlyCollection<CompetitionSummaryDto> ManagedCompetitions);

public class AppUserWriteDto
{
    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(11, MinimumLength = 11)]
    [RegularExpression("^[0-9]*$")]
    public string OIB { get; set; } = string.Empty;

    [Required, StringLength(13, MinimumLength = 13)]
    [RegularExpression("^[0-9]*$")]
    public string JMBG { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public List<int> ManagedCompetitionIds { get; set; } = new();
}

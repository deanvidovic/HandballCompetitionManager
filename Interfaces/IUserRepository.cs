using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyList<UserSummaryViewModel>> GetAllAsync();

    Task<UserSummaryViewModel?> GetByIdAsync(int id);

    Task<UserSummaryViewModel?> GetAssignedRefereeByMatchIdAsync(int matchId);

    Task<IReadOnlyList<int>> GetAssignedMatchIdsByUserIdAsync(int userId);

}

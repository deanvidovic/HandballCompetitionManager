using HandballCompetitionManager.ViewModels;

namespace HandballCompetitionManager.Interfaces;

public interface IMatchEventRepository
{
    Task<IReadOnlyList<MatchEventSummaryViewModel>> GetAllAsync();

    Task<IReadOnlyList<MatchEventSummaryViewModel>> GetByMatchIdAsync(int matchId);
}

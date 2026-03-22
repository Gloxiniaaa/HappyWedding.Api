using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Milestone;

namespace HappyWedding.Api.Services;

public interface IMilestoneService
{
    Task<List<Milestone>> GetMilestonesAsync(string userId);
    Task<Milestone?> AddMilestoneAsync(string userId, CreateMilestoneDto dto);
    Task<Milestone?> UpdateMilestoneAsync(string userId, Guid milestoneId, UpdateMilestoneDto dto);
    Task<Milestone?> ToggleCompletedAsync(string userId, Guid milestoneId);
    Task<bool> DeleteMilestoneAsync(string userId, Guid milestoneId);
}

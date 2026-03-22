using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Milestone;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Services;

public class MilestoneService(HappyWeddingDbContext db) : IMilestoneService
{
    private readonly IWeddingService _weddingService = new WeddingService(db);

    public async Task<List<Milestone>> GetMilestonesAsync(string userId)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return new List<Milestone>();
        }

        return await db.Milestones
            .Where(m => m.WeddingId == wedding.Id)
            .ToListAsync();
    }

    public async Task<Milestone?> AddMilestoneAsync(string userId, CreateMilestoneDto dto)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return null;
        }

        var milestone = new Milestone
        {
            WeddingId = wedding.Id,
            Title = dto.Title.Trim(),
            Subtitle = dto.Subtitle.Trim(),
            Date = dto.Date,
            Completed = dto.Completed,
            Emoji = dto.Emoji.Trim()
        };

        db.Milestones.Add(milestone);
        await db.SaveChangesAsync();
        return milestone;
    }

    public async Task<Milestone?> UpdateMilestoneAsync(string userId, Guid milestoneId, UpdateMilestoneDto dto)
    {
        var milestone = await GetMilestoneWithOwnershipCheckAsync(userId, milestoneId);
        if (milestone == null)
        {
            return null;
        }

        milestone.Title = dto.Title.Trim();
        milestone.Subtitle = dto.Subtitle.Trim();
        milestone.Date = dto.Date;
        milestone.Completed = dto.Completed;
        milestone.Emoji = dto.Emoji.Trim();

        db.Milestones.Update(milestone);
        await db.SaveChangesAsync();
        return milestone;
    }

    public async Task<Milestone?> ToggleCompletedAsync(string userId, Guid milestoneId)
    {
        var milestone = await GetMilestoneWithOwnershipCheckAsync(userId, milestoneId);
        if (milestone == null)
        {
            return null;
        }

        milestone.Completed = !milestone.Completed;

        db.Milestones.Update(milestone);
        await db.SaveChangesAsync();
        return milestone;
    }

    public async Task<bool> DeleteMilestoneAsync(string userId, Guid milestoneId)
    {
        var milestone = await GetMilestoneWithOwnershipCheckAsync(userId, milestoneId);
        if (milestone == null)
        {
            return false;
        }

        db.Milestones.Remove(milestone);
        await db.SaveChangesAsync();
        return true;
    }

    private async Task<Milestone?> GetMilestoneWithOwnershipCheckAsync(string userId, Guid milestoneId)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return null;
        }

        return await db.Milestones
            .Where(m => m.Id == milestoneId && m.WeddingId == wedding.Id)
            .FirstOrDefaultAsync();
    }
}

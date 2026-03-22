using HappyWedding.Api.Data;
using HappyWedding.Api.DTOs.Wedding;
using HappyWedding.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Services;

public class WeddingService(HappyWeddingDbContext db) : IWeddingService
{
    public async Task<Wedding?> GetMyWeddingAsync(string userId)
    {
        return await db.Weddings
            .Where(w => w.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<Wedding?> CreateWeddingAsync(string userId, CreateWeddingDto dto)
    {
        // Check if wedding already exists for this user
        var existingWedding = await GetMyWeddingAsync(userId);
        if (existingWedding != null)
        {
            return null;
        }

        var wedding = new Wedding
        {
            UserId = userId,
            Name1 = dto.Name1.Trim(),
            Name2 = dto.Name2.Trim(),
            Date = dto.Date,
            Location = dto.Location.Trim(),
            Tagline = dto.Tagline.Trim()
        };

        db.Weddings.Add(wedding);
        await db.SaveChangesAsync();
        return wedding;
    }

    public async Task<Wedding?> UpdateWeddingAsync(string userId, UpdateWeddingDto dto)
    {
        var wedding = await GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return null;
        }

        wedding.Name1 = dto.Name1.Trim();
        wedding.Name2 = dto.Name2.Trim();
        wedding.Date = dto.Date;
        wedding.Location = dto.Location.Trim();
        wedding.Tagline = dto.Tagline.Trim();

        db.Weddings.Update(wedding);
        await db.SaveChangesAsync();
        return wedding;
    }

    public async Task<bool> DeleteWeddingAsync(string userId)
    {
        var wedding = await GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return false;
        }

        db.Weddings.Remove(wedding);
        await db.SaveChangesAsync();
        return true;
    }
}

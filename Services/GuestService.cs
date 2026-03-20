using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Guest;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Services;

public class GuestService(HappyWeddingDbContext db) : IGuestService
{
    private readonly IWeddingService _weddingService = new WeddingService(db);

    public async Task<List<Guest>> GetGuestsAsync(string userId)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return new List<Guest>();
        }

        return await db.Guests
            .Where(g => g.WeddingId == wedding.Id)
            .ToListAsync();
    }

    public async Task<Guest?> AddGuestAsync(string userId, CreateGuestDto dto)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return null;
        }

        var guest = new Guest
        {
            WeddingId = wedding.Id,
            Name = dto.Name.Trim(),
            Note = dto.Note?.Trim(),
            SeatCount = Math.Max(1, dto.SeatCount),
            Side = dto.Side,
            Confirmed = false,
            CreatedAt = DateTime.UtcNow
        };

        db.Guests.Add(guest);
        await db.SaveChangesAsync();
        return guest;
    }

    public async Task<Guest?> UpdateGuestAsync(string userId, Guid guestId, UpdateGuestDto dto)
    {
        var guest = await GetGuestWithOwnershipCheckAsync(userId, guestId);
        if (guest == null)
        {
            return null;
        }

        guest.Name = dto.Name.Trim();
        guest.Note = dto.Note?.Trim();
        guest.SeatCount = Math.Max(1, dto.SeatCount);
        guest.UpdatedAt = DateTime.UtcNow;

        db.Guests.Update(guest);
        await db.SaveChangesAsync();
        return guest;
    }

    public async Task<Guest?> ToggleConfirmedAsync(string userId, Guid guestId)
    {
        var guest = await GetGuestWithOwnershipCheckAsync(userId, guestId);
        if (guest == null)
        {
            return null;
        }

        guest.Confirmed = !guest.Confirmed;
        guest.UpdatedAt = DateTime.UtcNow;

        db.Guests.Update(guest);
        await db.SaveChangesAsync();
        return guest;
    }

    public async Task<bool> DeleteGuestAsync(string userId, Guid guestId)
    {
        var guest = await GetGuestWithOwnershipCheckAsync(userId, guestId);
        if (guest == null)
        {
            return false;
        }

        db.Guests.Remove(guest);
        await db.SaveChangesAsync();
        return true;
    }

    private async Task<Guest?> GetGuestWithOwnershipCheckAsync(string userId, Guid guestId)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return null;
        }

        return await db.Guests
            .Where(g => g.Id == guestId && g.WeddingId == wedding.Id)
            .FirstOrDefaultAsync();
    }
}

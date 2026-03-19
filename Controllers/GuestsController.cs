using System.Security.Claims;
using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Guest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Controllers;

[Route("api/wedding/guests")]
[ApiController]
[Authorize]
public class GuestController(HappyWeddingDbContext db) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    private async Task<Wedding?> GetOwnedWeddingAsync() =>
        await db.Weddings.FirstOrDefaultAsync(w => w.UserId == CurrentUserId);

    // GET api/wedding/guests
    [HttpGet]
    public async Task<IActionResult> GetGuests()
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var guests = await db.Guests
            .Where(g => g.WeddingId == wedding.Id)
            .Select(g => MapToResponse(g))
            .ToListAsync();

        return Ok(guests);
    }

    // POST api/wedding/guests
    [HttpPost]
    public async Task<IActionResult> AddGuest([FromBody] CreateGuestDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var guest = new Guest
        {
            WeddingId = wedding.Id,
            Name = dto.Name.Trim(),
            Note = string.IsNullOrWhiteSpace(dto.Note) ? null : dto.Note.Trim(),
            SeatCount = Math.Max(1, dto.SeatCount),
            Side = dto.Side,
        };

        db.Guests.Add(guest);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGuests), MapToResponse(guest));
    }

    // PUT api/wedding/guests/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateGuest(Guid id, [FromBody] UpdateGuestDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var guest = await db.Guests
            .FirstOrDefaultAsync(g => g.Id == id && g.WeddingId == wedding.Id);
        if (guest is null) return NotFound(new { message = "Guest not found." });

        guest.Name = dto.Name.Trim();
        guest.Note = string.IsNullOrWhiteSpace(dto.Note) ? null : dto.Note.Trim();
        guest.SeatCount = Math.Max(1, dto.SeatCount);

        await db.SaveChangesAsync();
        return Ok(MapToResponse(guest));
    }

    // PATCH api/wedding/guests/{id}/toggle
    [HttpPatch("{id:guid}/toggle")]
    public async Task<IActionResult> ToggleConfirmed(Guid id)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var guest = await db.Guests
            .FirstOrDefaultAsync(g => g.Id == id && g.WeddingId == wedding.Id);
        if (guest is null) return NotFound(new { message = "Guest not found." });

        guest.Confirmed = !guest.Confirmed;
        await db.SaveChangesAsync();
        return Ok(MapToResponse(guest));
    }

    // DELETE api/wedding/guests/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGuest(Guid id)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var guest = await db.Guests
            .FirstOrDefaultAsync(g => g.Id == id && g.WeddingId == wedding.Id);
        if (guest is null) return NotFound(new { message = "Guest not found." });

        db.Guests.Remove(guest);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static GuestResponseDto MapToResponse(Guest g) => new()
    {
        Id = g.Id,
        Name = g.Name,
        Note = g.Note,
        SeatCount = g.SeatCount,
        Confirmed = g.Confirmed,
        Side = g.Side,
        WeddingId = g.WeddingId,
    };
}
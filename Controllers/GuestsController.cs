using System.Security.Claims;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Guest;
using HappyWedding.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWedding.Api.Controllers;

[Route("api/wedding/guests")]
[ApiController]
[Authorize]
public class GuestController(IGuestService guestService) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    // GET api/wedding/guests
    [HttpGet]
    public async Task<IActionResult> GetGuests()
    {
        var userId = CurrentUserId;
        var guests = await guestService.GetGuestsAsync(userId);

        return Ok(guests.Select(MapToResponse).ToList());
    }

    // POST api/wedding/guests
    [HttpPost]
    public async Task<IActionResult> AddGuest([FromBody] CreateGuestDto dto)
    {
        var userId = CurrentUserId;
        var guest = await guestService.AddGuestAsync(userId, dto);

        if (guest is null)
            return NotFound(new { message = "No wedding found." });

        return CreatedAtAction(nameof(GetGuests), MapToResponse(guest));
    }

    // PUT api/wedding/guests/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateGuest(Guid id, [FromBody] UpdateGuestDto dto)
    {
        var userId = CurrentUserId;
        var guest = await guestService.UpdateGuestAsync(userId, id, dto);

        if (guest is null)
            return NotFound(new { message = "Guest not found." });

        return Ok(MapToResponse(guest));
    }

    // PATCH api/wedding/guests/{id}/toggle
    [HttpPatch("{id:guid}/toggle")]
    public async Task<IActionResult> ToggleConfirmed(Guid id)
    {
        var userId = CurrentUserId;
        var guest = await guestService.ToggleConfirmedAsync(userId, id);

        if (guest is null)
            return NotFound(new { message = "Guest not found." });

        return Ok(MapToResponse(guest));
    }

    // DELETE api/wedding/guests/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGuest(Guid id)
    {
        var userId = CurrentUserId;
        var deleted = await guestService.DeleteGuestAsync(userId, id);

        if (!deleted)
            return NotFound(new { message = "Guest not found." });

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
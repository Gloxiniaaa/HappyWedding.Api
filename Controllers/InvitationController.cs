using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Dtos.Invitation;
using HappyWedding.Api.Models.Dtos.Milestone;
using HappyWedding.Api.Models.Dtos.Photo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Controllers;

[ApiController]
[Route("api/invitation")]
[AllowAnonymous]                    // ← public endpoint – no login required
public class InvitationController(HappyWeddingDbContext db) : ControllerBase
{
    /// <summary>
    /// Get wedding information visible to guests (invitation view)
    /// </summary>
    /// <param name="weddingId">The ID of the wedding</param>
    /// <response code="200">Wedding details including public milestones</response>
    /// <response code="404">Wedding not found</response>
    [HttpGet("{weddingId:guid}")]
    [ProducesResponseType<WeddingForGuestDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeddingForGuestDto>> GetInvitation(Guid weddingId)
    {
        var wedding = await db.Weddings
            .AsNoTracking()
            .Where(w => w.Id == weddingId)
            .Select(w => new WeddingForGuestDto
            {
                Id = w.Id,
                Name1 = w.Name1,
                Name2 = w.Name2,
                Date = w.Date,
                Location = w.Location,
                Tagline = w.Tagline,

                Milestones = w.Milestones
                    .Select(m => new MilestoneResponseDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Subtitle = m.Subtitle,
                        Date = m.Date,
                        Completed = m.Completed,
                        Emoji = m.Emoji,
                        WeddingId = m.WeddingId
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (wedding is null)
        {
            return NotFound();
        }

        return Ok(wedding);
    }

    // GET api/invitation/{weddingId}/photos
    [HttpGet("{weddingId:guid}/photos")]
    public async Task<IActionResult> GetInvitationPhotos(Guid weddingId)
    {
        var exists = await db.Weddings.AnyAsync(w => w.Id == weddingId);
        if (!exists) return NotFound();

        var photos = await db.WeddingPhotos
            .AsNoTracking()
            .Where(p => p.WeddingId == weddingId)
            .Select(p => new PhotoResponseDto
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                AspectRatio = p.AspectRatio,
                Caption = p.Caption,
            })
            .ToListAsync();

        return Ok(photos);
    }
}
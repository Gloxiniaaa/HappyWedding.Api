using System.Security.Claims;
using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Milestone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Controllers;

[Route("api/wedding/milestones")]
[ApiController]
[Authorize]
public class MilestoneController(HappyWeddingDbContext db) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    private async Task<Wedding?> GetOwnedWeddingAsync() =>
        await db.Weddings.FirstOrDefaultAsync(w => w.UserId == CurrentUserId);

    // GET api/wedding/milestones
    [HttpGet]
    public async Task<IActionResult> GetMilestones()
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        var milestones = await db.Milestones
            .Where(m => m.WeddingId == wedding.Id)
            .Select(m => MapToResponse(m))
            .ToListAsync();

        return Ok(milestones);
    }

    // POST api/wedding/milestones
    [HttpPost]
    public async Task<IActionResult> AddMilestone([FromBody] CreateMilestoneDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        var milestone = new Milestone
        {
            WeddingId = wedding.Id,
            Title = dto.Title,
            Subtitle = dto.Subtitle,
            Date = dto.Date,
            Completed = dto.Completed,
            Emoji = dto.Emoji,
        };

        db.Milestones.Add(milestone);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMilestones), MapToResponse(milestone));
    }

    // PUT api/wedding/milestones/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMilestone(Guid id, [FromBody] UpdateMilestoneDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        var milestone = await db.Milestones
            .FirstOrDefaultAsync(m => m.Id == id && m.WeddingId == wedding.Id);

        if (milestone is null)
            return NotFound(new { message = "Milestone not found." });

        milestone.Title = dto.Title;
        milestone.Subtitle = dto.Subtitle;
        milestone.Date = dto.Date;
        milestone.Completed = dto.Completed;
        milestone.Emoji = dto.Emoji;

        await db.SaveChangesAsync();
        return Ok(MapToResponse(milestone));
    }

    // DELETE api/wedding/milestones/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMilestone(Guid id)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        var milestone = await db.Milestones
            .FirstOrDefaultAsync(m => m.Id == id && m.WeddingId == wedding.Id);

        if (milestone is null)
            return NotFound(new { message = "Milestone not found." });

        db.Milestones.Remove(milestone);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Mapper ────────────────────────────────────────────────────────────────
    private static MilestoneResponseDto MapToResponse(Milestone m) => new()
    {
        Id = m.Id,
        Title = m.Title,
        Subtitle = m.Subtitle,
        Date = m.Date,
        Completed = m.Completed,
        Emoji = m.Emoji,
        WeddingId = m.WeddingId,
    };
}
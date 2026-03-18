using System.Security.Claims;
using HappyWedding.Api.Data;
using HappyWedding.Api.DTOs.Milestone;
using HappyWedding.Api.DTOs.Wedding;
using HappyWedding.Api.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Controllers;

[Route("api/wedding")]
[ApiController]
[Authorize]
public class WeddingController(HappyWeddingDbContext db) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    // GET api/wedding
    [HttpGet]
    public async Task<IActionResult> GetMyWedding()
    {
        var userId = CurrentUserId;

        var wedding = await db.Weddings
            .Include(w => w.Milestones)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        return Ok(MapToResponse(wedding));
    }

    [HttpPost]
    public async Task<IActionResult> CreateWedding([FromBody] CreateWeddingDto dto)
    {
        var userId = CurrentUserId;

        var exists = await db.Weddings.AnyAsync(w => w.UserId == userId);
        if (exists)
            return Conflict(new { message = "You already have a wedding." });

        var wedding = new Wedding
        {
            UserId = userId,
            Name1 = dto.Name1,
            Name2 = dto.Name2,
            Date = dto.Date,
            Location = dto.Location,
            Tagline = dto.Tagline,
        };

        db.Weddings.Add(wedding);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMyWedding), MapToResponse(wedding));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateWedding([FromBody] UpdateWeddingDto dto)
    {
        var userId = CurrentUserId;

        var wedding = await db.Weddings
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        wedding.Name1 = dto.Name1;
        wedding.Name2 = dto.Name2;
        wedding.Date = dto.Date;
        wedding.Location = dto.Location;
        wedding.Tagline = dto.Tagline;

        await db.SaveChangesAsync();
        return Ok(MapToResponse(wedding));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteWedding()
    {
        var userId = CurrentUserId;

        var wedding = await db.Weddings
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        db.Weddings.Remove(wedding);
        await db.SaveChangesAsync();
        return NoContent();
    }
    
    // ── Mapper ────────────────────────────────────────────────────────────────
    private static WeddingResponseDto MapToResponse(Wedding w) => new()
    {
        Id = w.Id,
        Name1 = w.Name1,
        Name2 = w.Name2,
        Date = w.Date,
        Location = w.Location,
        Tagline = w.Tagline,
        // Milestones = w.Milestones.Select(m => new MilestoneResponseDto
        // {
        //     Id        = m.Id,
        //     Title     = m.Title,
        //     Subtitle  = m.Subtitle,
        //     Date      = m.Date,
        //     Completed = m.Completed,
        //     Emoji     = m.Emoji,
        //     WeddingId = m.WeddingId,
        // }).ToList()
    };
}
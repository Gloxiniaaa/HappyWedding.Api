using System.Security.Claims;
using HappyWedding.Api.DTOs.Wedding;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWedding.Api.Controllers;

[Route("api/wedding")]
[ApiController]
[Authorize]
public class WeddingController(IWeddingService weddingService) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    // GET api/wedding
    [HttpGet]
    public async Task<IActionResult> GetMyWedding()
    {
        var userId = CurrentUserId;
        var wedding = await weddingService.GetMyWeddingAsync(userId);

        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        return Ok(MapToResponse(wedding));
    }

    [HttpPost]
    public async Task<IActionResult> CreateWedding([FromBody] CreateWeddingDto dto)
    {
        var userId = CurrentUserId;
        var wedding = await weddingService.CreateWeddingAsync(userId, dto);

        if (wedding is null)
            return Conflict(new { message = "You already have a wedding." });

        return CreatedAtAction(nameof(GetMyWedding), MapToResponse(wedding));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateWedding([FromBody] UpdateWeddingDto dto)
    {
        var userId = CurrentUserId;
        var wedding = await weddingService.UpdateWeddingAsync(userId, dto);

        if (wedding is null)
            return NotFound(new { message = "No wedding found." });

        return Ok(MapToResponse(wedding));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteWedding()
    {
        var userId = CurrentUserId;
        var deleted = await weddingService.DeleteWeddingAsync(userId);

        if (!deleted)
            return NotFound(new { message = "No wedding found." });

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
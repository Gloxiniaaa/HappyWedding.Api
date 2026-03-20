using System.Security.Claims;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Milestone;
using HappyWedding.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWedding.Api.Controllers;

[Route("api/wedding/milestones")]
[ApiController]
[Authorize]
public class MilestoneController(IMilestoneService milestoneService) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    // GET api/wedding/milestones
    [HttpGet]
    public async Task<IActionResult> GetMilestones()
    {
        var userId = CurrentUserId;
        var milestones = await milestoneService.GetMilestonesAsync(userId);

        return Ok(milestones.Select(MapToResponse).ToList());
    }

    // POST api/wedding/milestones
    [HttpPost]
    public async Task<IActionResult> AddMilestone([FromBody] CreateMilestoneDto dto)
    {
        var userId = CurrentUserId;
        var milestone = await milestoneService.AddMilestoneAsync(userId, dto);

        if (milestone is null)
            return NotFound(new { message = "No wedding found." });

        return CreatedAtAction(nameof(GetMilestones), MapToResponse(milestone));
    }

    // PUT api/wedding/milestones/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMilestone(Guid id, [FromBody] UpdateMilestoneDto dto)
    {
        var userId = CurrentUserId;
        var milestone = await milestoneService.UpdateMilestoneAsync(userId, id, dto);

        if (milestone is null)
            return NotFound(new { message = "Milestone not found." });

        return Ok(MapToResponse(milestone));
    }

    // PATCH api/wedding/milestones/{id}/toggle
    [HttpPatch("{id:guid}/toggle")]
    public async Task<IActionResult> ToggleCompleted(Guid id)
    {
        var userId = CurrentUserId;
        var milestone = await milestoneService.ToggleCompletedAsync(userId, id);

        if (milestone is null)
            return NotFound(new { message = "Milestone not found." });

        return Ok(MapToResponse(milestone));
    }

    // DELETE api/wedding/milestones/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMilestone(Guid id)
    {
        var userId = CurrentUserId;
        var deleted = await milestoneService.DeleteMilestoneAsync(userId, id);

        if (!deleted)
            return NotFound(new { message = "Milestone not found." });

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
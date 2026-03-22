using System.Security.Claims;
using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Photo;
using HappyWedding.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Controllers;

[Route("api/wedding/photos")]
[ApiController]
[Authorize]
public class PhotoGalleryController(HappyWeddingDbContext db, ICloudinaryService cloudinary) : ControllerBase
{
    private static readonly HashSet<string> AllowedRatios = ["1:1", "4:3", "3:4", "16:9", "9:16"];

    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    private async Task<Wedding?> GetOwnedWeddingAsync() =>
        await db.Weddings.FirstOrDefaultAsync(w => w.UserId == CurrentUserId);

    // GET api/wedding/photos
    [HttpGet]
    public async Task<IActionResult> GetPhotos()
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var photos = await db.WeddingPhotos
            .Where(p => p.WeddingId == wedding.Id)
            .Select(p => MapToResponse(p))
            .ToListAsync();

        return Ok(photos);
    }

    // POST api/wedding/photos
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        if (!AllowedRatios.Contains(dto.AspectRatio))
            return BadRequest(new { message = $"Invalid aspect ratio. Allowed: {string.Join(", ", AllowedRatios)}" });

        var uploaded = await cloudinary.UploadImageAsync(dto.File, folder: $"weddings/{wedding.Id}");

        var photo = new WeddingPhoto
        {
            WeddingId = wedding.Id,
            PublicId = uploaded.PublicId,
            ImageUrl = uploaded.SecureUrl,
            AspectRatio = dto.AspectRatio,
            Caption = dto.Caption?.Trim(),
        };

        db.WeddingPhotos.Add(photo);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPhotos), MapToResponse(photo));
    }

    // PUT api/wedding/photos/{id}  — update metadata only, no re-upload
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePhoto(Guid id, [FromBody] UpdatePhotoDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        if (!AllowedRatios.Contains(dto.AspectRatio))
            return BadRequest(new { message = $"Invalid aspect ratio. Allowed: {string.Join(", ", AllowedRatios)}" });

        var photo = await db.WeddingPhotos
            .FirstOrDefaultAsync(p => p.Id == id && p.WeddingId == wedding.Id);
        if (photo is null) return NotFound(new { message = "Photo not found." });

        photo.AspectRatio = dto.AspectRatio;
        photo.Caption = dto.Caption?.Trim();

        await db.SaveChangesAsync();
        return Ok(MapToResponse(photo));
    }

    // DELETE api/wedding/photos/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var photo = await db.WeddingPhotos
            .FirstOrDefaultAsync(p => p.Id == id && p.WeddingId == wedding.Id);
        if (photo is null) return NotFound(new { message = "Photo not found." });

        await cloudinary.DeleteImageAsync(photo.PublicId); // remove from Cloudinary first
        db.WeddingPhotos.Remove(photo);
        await db.SaveChangesAsync();

        return NoContent();
    }

    private static PhotoResponseDto MapToResponse(WeddingPhoto p) => new()
    {
        Id = p.Id,
        ImageUrl = p.ImageUrl,
        AspectRatio = p.AspectRatio,
        Caption = p.Caption,
    };
}
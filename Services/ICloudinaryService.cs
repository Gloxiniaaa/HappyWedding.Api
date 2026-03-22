using HappyWedding.Api.Models.Domain;

namespace HappyWedding.Api.Services;

public interface ICloudinaryService
{
    Task<ImageUploadResult> UploadImageAsync(IFormFile file, string? folder = null);
    Task<bool> DeleteImageAsync(string publicId);
    Task<IEnumerable<ImageUploadResult>> UploadMultipleImagesAsync(IList<IFormFile> files, string? folder = null);
}
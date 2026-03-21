namespace HappyWedding.Api.Models.Dtos.Photo;

public class UploadPhotoDto
{
    public IFormFile File { get; set; } = null!;
    public string AspectRatio { get; set; } = "1:1";
    public string? Caption { get; set; }
}
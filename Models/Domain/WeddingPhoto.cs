// Models/Domain/WeddingPhoto.cs

namespace HappyWedding.Api.Models.Domain;

public class WeddingPhoto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PublicId { get; set; } = string.Empty;   // Cloudinary public ID (for deletion)
    public string ImageUrl { get; set; } = string.Empty;   // Cloudinary delivery URL
    public string AspectRatio { get; set; } = "1:1";       // "1:1" | "4:3" | "3:4" | "16:9" | "9:16"
    public string? Caption { get; set; }

    public Guid WeddingId { get; set; }
    public Wedding Wedding { get; set; } = null!;
}
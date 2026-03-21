namespace HappyWedding.Api.Models.Domain;

public class ImageUploadResult
{
    public string PublicId { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string SecureUrl { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public long Bytes { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
namespace HappyWedding.Api.Models.Dtos.Photo;

public class PhotoResponseDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string AspectRatio { get; set; } = string.Empty;
    public string? Caption { get; set; }
}
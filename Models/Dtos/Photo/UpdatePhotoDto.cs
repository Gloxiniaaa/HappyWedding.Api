namespace HappyWedding.Api.Models.Dtos.Photo;

public class UpdatePhotoDto
{
    public string AspectRatio { get; set; } = "1:1";
    public string? Caption { get; set; }
}
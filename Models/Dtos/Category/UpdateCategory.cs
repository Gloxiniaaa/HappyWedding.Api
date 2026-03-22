namespace HappyWedding.Api.Models.Dtos.Category;

public record UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
}
namespace HappyWedding.Api.Models.Dtos.Category;

public record CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
}
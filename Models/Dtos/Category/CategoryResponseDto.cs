using HappyWedding.Api.Models.Dtos.Expense;

namespace HappyWedding.Api.Models.Dtos.Category;

public record CategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public List<ExpenseResponseDto> Expenses { get; set; } = new();
}
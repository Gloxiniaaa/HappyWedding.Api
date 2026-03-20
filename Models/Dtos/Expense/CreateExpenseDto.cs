namespace HappyWedding.Api.Models.Dtos.Expense;

public record CreateExpenseDto
{
    public string Name { get; set; } = string.Empty;
    public long EstimateCost { get; set; }
    public long ActualCost { get; set; }
    public bool Paid { get; set; } = false;
}
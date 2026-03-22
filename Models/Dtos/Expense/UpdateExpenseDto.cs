namespace HappyWedding.Api.Models.Dtos.Expense;

public record UpdateExpenseDto
{
    public string Name { get; set; } = string.Empty;
    public long EstimateCost { get; set; }
    public long ActualCost { get; set; }
    public bool Paid { get; set; }
}
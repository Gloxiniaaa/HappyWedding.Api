namespace HappyWedding.Api.Models.Dtos.Expense;

public class ExpenseResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long EstimateCost { get; set; }
    public long ActualCost { get; set; }
    public bool Paid { get; set; }
    public Guid CategoryId { get; set; }
}
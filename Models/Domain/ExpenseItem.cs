namespace HappyWedding.Api.Models.Domain;


public class ExpenseItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public long EstimateCost { get; set; }   // VND — use long, not decimal
    public long ActualCost { get; set; }
    public bool Paid { get; set; } = false;

    public Guid CategoryId { get; set; }
    public ExpenseCategory Category { get; set; } = null!;
}
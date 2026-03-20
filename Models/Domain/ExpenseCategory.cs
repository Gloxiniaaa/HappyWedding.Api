namespace HappyWedding.Api.Models.Domain;

public class ExpenseCategory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;

    public Guid WeddingId { get; set; }
    public Wedding Wedding { get; set; } = null!;
    public ICollection<ExpenseItem> Expenses { get; set; } = new List<ExpenseItem>();
}
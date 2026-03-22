using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Category;
using HappyWedding.Api.Models.Dtos.Expense;

namespace HappyWedding.Api.Services;

public interface IExpenseService
{
    Task<List<ExpenseCategory>> GetCategoriesAsync(string userId);
    Task<ExpenseCategory?> AddCategoryAsync(string userId, CreateCategoryDto dto);
    Task<ExpenseCategory?> UpdateCategoryAsync(string userId, Guid categoryId, UpdateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(string userId, Guid categoryId);
    Task<List<ExpenseItem>> GetExpensesAsync(string userId, Guid categoryId);
    Task<ExpenseItem?> AddExpenseAsync(string userId, Guid categoryId, CreateExpenseDto dto);
    Task<ExpenseItem?> UpdateExpenseAsync(string userId, Guid expenseId, UpdateExpenseDto dto);
    Task<ExpenseItem?> TogglePaidAsync(string userId, Guid expenseId);
    Task<bool> DeleteExpenseAsync(string userId, Guid expenseId);
}

using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Category;
using HappyWedding.Api.Models.Dtos.Expense;
using Microsoft.EntityFrameworkCore;

namespace HappyWedding.Api.Services;

public class ExpenseService(HappyWeddingDbContext db) : IExpenseService
{
    private readonly IWeddingService _weddingService = new WeddingService(db);

    public async Task<List<ExpenseCategory>> GetCategoriesAsync(string userId)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return new List<ExpenseCategory>();
        }

        return await db.ExpenseCategories
            .Where(c => c.WeddingId == wedding.Id)
            .Include(c => c.Expenses)
            .ToListAsync();
    }

    public async Task<ExpenseCategory?> AddCategoryAsync(string userId, CreateCategoryDto dto)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return null;
        }

        var category = new ExpenseCategory
        {
            WeddingId = wedding.Id,
            Name = dto.Name.Trim(),
            Emoji = dto.Emoji.Trim()
        };

        db.ExpenseCategories.Add(category);
        await db.SaveChangesAsync();
        return category;
    }

    public async Task<ExpenseCategory?> UpdateCategoryAsync(string userId, Guid categoryId, UpdateCategoryDto dto)
    {
        var category = await GetCategoryWithOwnershipCheckAsync(userId, categoryId);
        if (category == null)
        {
            return null;
        }

        category.Name = dto.Name.Trim();
        category.Emoji = dto.Emoji.Trim();

        db.ExpenseCategories.Update(category);
        await db.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteCategoryAsync(string userId, Guid categoryId)
    {
        var category = await GetCategoryWithOwnershipCheckAsync(userId, categoryId);
        if (category == null)
        {
            return false;
        }

        db.ExpenseCategories.Remove(category);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ExpenseItem>> GetExpensesAsync(string userId, Guid categoryId)
    {
        var category = await GetCategoryWithOwnershipCheckAsync(userId, categoryId);
        if (category == null)
        {
            return new List<ExpenseItem>();
        }

        return await db.ExpenseItems
            .Where(e => e.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<ExpenseItem?> AddExpenseAsync(string userId, Guid categoryId, CreateExpenseDto dto)
    {
        var category = await GetCategoryWithOwnershipCheckAsync(userId, categoryId);
        if (category == null)
        {
            return null;
        }

        var expense = new ExpenseItem
        {
            CategoryId = categoryId,
            Name = dto.Name.Trim(),
            EstimateCost = dto.EstimateCost,
            ActualCost = dto.ActualCost,
            Paid = dto.Paid
        };

        db.ExpenseItems.Add(expense);
        await db.SaveChangesAsync();
        return expense;
    }

    public async Task<ExpenseItem?> UpdateExpenseAsync(string userId, Guid expenseId, UpdateExpenseDto dto)
    {
        var expense = await GetExpenseWithOwnershipCheckAsync(userId, expenseId);
        if (expense == null)
        {
            return null;
        }

        expense.Name = dto.Name.Trim();
        expense.EstimateCost = dto.EstimateCost;
        expense.ActualCost = dto.ActualCost;
        expense.Paid = dto.Paid;

        db.ExpenseItems.Update(expense);
        await db.SaveChangesAsync();
        return expense;
    }

    public async Task<ExpenseItem?> TogglePaidAsync(string userId, Guid expenseId)
    {
        var expense = await GetExpenseWithOwnershipCheckAsync(userId, expenseId);
        if (expense == null)
        {
            return null;
        }

        expense.Paid = !expense.Paid;

        db.ExpenseItems.Update(expense);
        await db.SaveChangesAsync();
        return expense;
    }

    public async Task<bool> DeleteExpenseAsync(string userId, Guid expenseId)
    {
        var expense = await GetExpenseWithOwnershipCheckAsync(userId, expenseId);
        if (expense == null)
        {
            return false;
        }

        db.ExpenseItems.Remove(expense);
        await db.SaveChangesAsync();
        return true;
    }

    private async Task<ExpenseCategory?> GetCategoryWithOwnershipCheckAsync(string userId, Guid categoryId)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return null;
        }

        return await db.ExpenseCategories
            .Where(c => c.Id == categoryId && c.WeddingId == wedding.Id)
            .FirstOrDefaultAsync();
    }

    private async Task<ExpenseItem?> GetExpenseWithOwnershipCheckAsync(string userId, Guid expenseId)
    {
        var wedding = await _weddingService.GetMyWeddingAsync(userId);
        if (wedding == null)
        {
            return null;
        }

        return await db.ExpenseItems
            .Where(e => e.Id == expenseId && e.Category.WeddingId == wedding.Id)
            .Include(e => e.Category)
            .FirstOrDefaultAsync();
    }
}

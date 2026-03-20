using System.Security.Claims;
using HappyWedding.Api.Data;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Category;
using HappyWedding.Api.Models.Dtos.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HappyWedding.Api.Controllers;

[Route("api/wedding/expenses")]
[ApiController]
[Authorize]
public class ExpensesController(HappyWeddingDbContext db) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    private async Task<Wedding?> GetOwnedWeddingAsync() =>
        await db.Weddings.FirstOrDefaultAsync(w => w.UserId == CurrentUserId);

    // ── Categories ────────────────────────────────────────────────────────────

    // GET api/wedding/expenses/categories
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var categories = await db.ExpenseCategories
            .Where(c => c.WeddingId == wedding.Id)
            .Include(c => c.Expenses)
            .Select(c => MapCategoryToResponse(c))
            .ToListAsync();

        return Ok(categories);
    }

    // POST api/wedding/expenses/categories
    [HttpPost("categories")]
    public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var category = new ExpenseCategory
        {
            WeddingId = wedding.Id,
            Name = dto.Name.Trim(),
            Emoji = dto.Emoji.Trim(),
        };

        db.ExpenseCategories.Add(category);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategories), MapCategoryToResponse(category));
    }

    // PUT api/wedding/expenses/categories/{id}
    [HttpPut("categories/{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var category = await db.ExpenseCategories
            .FirstOrDefaultAsync(c => c.Id == id && c.WeddingId == wedding.Id);
        if (category is null) return NotFound(new { message = "Category not found." });

        category.Name = dto.Name.Trim();
        category.Emoji = dto.Emoji.Trim();

        await db.SaveChangesAsync();
        return Ok(MapCategoryToResponse(category));
    }

    // DELETE api/wedding/expenses/categories/{id}
    [HttpDelete("categories/{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var category = await db.ExpenseCategories
            .FirstOrDefaultAsync(c => c.Id == id && c.WeddingId == wedding.Id);
        if (category is null) return NotFound(new { message = "Category not found." });

        db.ExpenseCategories.Remove(category); // cascade deletes expenses
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Expenses ──────────────────────────────────────────────────────────────

    // POST api/wedding/expenses/categories/{categoryId}/items
    [HttpPost("categories/{categoryId:guid}/items")]
    public async Task<IActionResult> AddExpense(Guid categoryId, [FromBody] CreateExpenseDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var category = await db.ExpenseCategories
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.WeddingId == wedding.Id);
        if (category is null) return NotFound(new { message = "Category not found." });

        var expense = new ExpenseItem
        {
            CategoryId = categoryId,
            Name = dto.Name.Trim(),
            EstimateCost = dto.EstimateCost,
            ActualCost = dto.ActualCost,
            Paid = dto.Paid,
        };

        db.ExpenseItems.Add(expense);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategories), MapExpenseToResponse(expense));
    }

    // PUT api/wedding/expenses/items/{id}
    [HttpPut("items/{id:guid}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseDto dto)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var expense = await db.ExpenseItems
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id && e.Category.WeddingId == wedding.Id);
        if (expense is null) return NotFound(new { message = "Expense not found." });

        expense.Name = dto.Name.Trim();
        expense.EstimateCost = dto.EstimateCost;
        expense.ActualCost = dto.ActualCost;
        expense.Paid = dto.Paid;

        await db.SaveChangesAsync();
        return Ok(MapExpenseToResponse(expense));
    }

    // PATCH api/wedding/expenses/items/{id}/toggle
    [HttpPatch("items/{id:guid}/toggle")]
    public async Task<IActionResult> TogglePaid(Guid id)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var expense = await db.ExpenseItems
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id && e.Category.WeddingId == wedding.Id);
        if (expense is null) return NotFound(new { message = "Expense not found." });

        expense.Paid = !expense.Paid;
        await db.SaveChangesAsync();
        return Ok(MapExpenseToResponse(expense));
    }

    // DELETE api/wedding/expenses/items/{id}
    [HttpDelete("items/{id:guid}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var wedding = await GetOwnedWeddingAsync();
        if (wedding is null) return NotFound(new { message = "No wedding found." });

        var expense = await db.ExpenseItems
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id && e.Category.WeddingId == wedding.Id);
        if (expense is null) return NotFound(new { message = "Expense not found." });

        db.ExpenseItems.Remove(expense);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Mappers ───────────────────────────────────────────────────────────────

    private static CategoryResponseDto MapCategoryToResponse(ExpenseCategory c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Emoji = c.Emoji,
        Expenses = c.Expenses.Select(MapExpenseToResponse).ToList(),
    };

    private static ExpenseResponseDto MapExpenseToResponse(ExpenseItem e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        EstimateCost = e.EstimateCost,
        ActualCost = e.ActualCost,
        Paid = e.Paid,
        CategoryId = e.CategoryId,
    };
}
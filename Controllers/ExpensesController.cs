using System.Security.Claims;
using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Category;
using HappyWedding.Api.Models.Dtos.Expense;
using HappyWedding.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HappyWedding.Api.Controllers;

[Route("api/wedding/expenses")]
[ApiController]
[Authorize]
public class ExpensesController(IExpenseService expenseService) : ControllerBase
{
    private string CurrentUserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    // ── Categories ────────────────────────────────────────────────────────────

    // GET api/wedding/expenses/categories
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var userId = CurrentUserId;
        var categories = await expenseService.GetCategoriesAsync(userId);

        return Ok(categories.Select(MapCategoryToResponse).ToList());
    }

    // POST api/wedding/expenses/categories
    [HttpPost("categories")]
    public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto dto)
    {
        var userId = CurrentUserId;
        var category = await expenseService.AddCategoryAsync(userId, dto);

        if (category is null)
            return NotFound(new { message = "No wedding found." });

        return CreatedAtAction(nameof(GetCategories), MapCategoryToResponse(category));
    }

    // PUT api/wedding/expenses/categories/{id}
    [HttpPut("categories/{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto)
    {
        var userId = CurrentUserId;
        var category = await expenseService.UpdateCategoryAsync(userId, id, dto);

        if (category is null)
            return NotFound(new { message = "Category not found." });

        return Ok(MapCategoryToResponse(category));
    }

    // DELETE api/wedding/expenses/categories/{id}
    [HttpDelete("categories/{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var userId = CurrentUserId;
        var deleted = await expenseService.DeleteCategoryAsync(userId, id);

        if (!deleted)
            return NotFound(new { message = "Category not found." });

        return NoContent();
    }

    // ── Expenses ──────────────────────────────────────────────────────────────

    // POST api/wedding/expenses/categories/{categoryId}/items
    [HttpPost("categories/{categoryId:guid}/items")]
    public async Task<IActionResult> AddExpense(Guid categoryId, [FromBody] CreateExpenseDto dto)
    {
        var userId = CurrentUserId;
        var expense = await expenseService.AddExpenseAsync(userId, categoryId, dto);

        if (expense is null)
            return NotFound(new { message = "Category not found." });

        return CreatedAtAction(nameof(GetCategories), MapExpenseToResponse(expense));
    }

    // PUT api/wedding/expenses/items/{id}
    [HttpPut("items/{id:guid}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseDto dto)
    {
        var userId = CurrentUserId;
        var expense = await expenseService.UpdateExpenseAsync(userId, id, dto);

        if (expense is null)
            return NotFound(new { message = "Expense not found." });

        return Ok(MapExpenseToResponse(expense));
    }

    // PATCH api/wedding/expenses/items/{id}/toggle
    [HttpPatch("items/{id:guid}/toggle")]
    public async Task<IActionResult> TogglePaid(Guid id)
    {
        var userId = CurrentUserId;
        var expense = await expenseService.TogglePaidAsync(userId, id);

        if (expense is null)
            return NotFound(new { message = "Expense not found." });

        return Ok(MapExpenseToResponse(expense));
    }

    // DELETE api/wedding/expenses/items/{id}
    [HttpDelete("items/{id:guid}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        var userId = CurrentUserId;
        var deleted = await expenseService.DeleteExpenseAsync(userId, id);

        if (!deleted)
            return NotFound(new { message = "Expense not found." });

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
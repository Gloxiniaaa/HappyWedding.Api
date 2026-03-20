using FluentValidation;
using HappyWedding.Api.Models.Dtos.Expense;

namespace HappyWedding.Api.Validators.Expense;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Expense name is required")
            .Length(2, 100).WithMessage("Expense name must be between 2 and 100 characters");

        RuleFor(x => x.EstimateCost)
            .GreaterThan(0).WithMessage("Estimated cost must be greater than 0");

        RuleFor(x => x.ActualCost)
            .GreaterThan(0)
            .When(x => x.ActualCost.HasValue)
            .WithMessage("Actual cost must be greater than 0 if provided");
    }
}

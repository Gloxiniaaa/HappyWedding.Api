using FluentValidation;
using HappyWedding.Api.Models.Dtos.Category;

namespace HappyWedding.Api.Validators.Category;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .Length(2, 50).WithMessage("Category name must be between 2 and 50 characters");

        RuleFor(x => x.Emoji)
            .MaximumLength(2).WithMessage("Emoji must be 1-2 characters");
    }
}

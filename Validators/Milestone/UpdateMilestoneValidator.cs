using FluentValidation;
using HappyWedding.Api.Models.Dtos.Milestone;

namespace HappyWedding.Api.Validators.Milestone;

public class UpdateMilestoneValidator : AbstractValidator<UpdateMilestoneDto>
{
    public UpdateMilestoneValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Milestone title is required")
            .Length(2, 200).WithMessage("Title must be between 2 and 200 characters");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Milestone date is required")
            .LessThanOrEqualTo(DateTime.Today.AddYears(10)).WithMessage("Milestone date must be within 10 years");

        RuleFor(x => x.Emoji)
            .MaximumLength(2).WithMessage("Emoji must be 1-2 characters");

        RuleFor(x => x.Subtitle)
            .MaximumLength(500).WithMessage("Subtitle cannot exceed 500 characters");
    }
}

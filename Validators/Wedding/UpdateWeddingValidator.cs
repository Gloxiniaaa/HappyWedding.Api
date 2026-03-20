using FluentValidation;
using HappyWedding.Api.DTOs.Wedding;

namespace HappyWedding.Api.Validators.Wedding;

public class UpdateWeddingValidator : AbstractValidator<UpdateWeddingDto>
{
    public UpdateWeddingValidator()
    {
        RuleFor(x => x.Name1)
            .NotEmpty().WithMessage("Groom's name is required")
            .Length(2, 100).WithMessage("Groom's name must be between 2 and 100 characters");

        RuleFor(x => x.Name2)
            .NotEmpty().WithMessage("Bride's name is required")
            .Length(2, 100).WithMessage("Bride's name must be between 2 and 100 characters");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Wedding date is required")
            .GreaterThan(DateTime.Today).WithMessage("Wedding date must be in the future");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Wedding location is required")
            .Length(3, 300).WithMessage("Location must be between 3 and 300 characters");

        RuleFor(x => x.Tagline)
            .MaximumLength(500).WithMessage("Tagline cannot exceed 500 characters");
    }
}

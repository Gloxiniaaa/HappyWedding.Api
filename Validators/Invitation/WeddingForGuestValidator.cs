using FluentValidation;
using HappyWedding.Api.Models.Dtos.Invitation;

namespace HappyWedding.Api.Validators.Invitation;

public class WeddingForGuestValidator : AbstractValidator<WeddingForGuestDto>
{
    public WeddingForGuestValidator()
    {
        RuleFor(x => x.Name1)
            .NotEmpty().WithMessage("Groom's name is required")
            .Length(2, 100).WithMessage("Groom's name must be between 2 and 100 characters");

        RuleFor(x => x.Name2)
            .NotEmpty().WithMessage("Bride's name is required")
            .Length(2, 100).WithMessage("Bride's name must be between 2 and 100 characters");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Wedding date is required");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Wedding location is required")
            .Length(3, 300).WithMessage("Location must be between 3 and 300 characters");
    }
}

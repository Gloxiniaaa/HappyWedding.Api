using FluentValidation;
using HappyWedding.Api.Models.Dtos.Guest;

namespace HappyWedding.Api.Validators.Guest;

public class UpdateGuestValidator : AbstractValidator<UpdateGuestDto>
{
    public UpdateGuestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Guest name is required")
            .Length(2, 100).WithMessage("Guest name must be between 2 and 100 characters");

        RuleFor(x => x.SeatCount)
            .GreaterThan(0).WithMessage("Seat count must be at least 1")
            .LessThanOrEqualTo(50).WithMessage("Seat count cannot exceed 50");

        RuleFor(x => x.Side)
            .IsInEnum().WithMessage("Side must be either 'Groom' or 'Bride'");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Note cannot exceed 500 characters");
    }
}

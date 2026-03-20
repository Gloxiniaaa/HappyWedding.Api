using FluentValidation;
using HappyWedding.Api.Models.Dtos.Auth;

namespace HappyWedding.Api.Validators.Auth;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("UserId is required");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("RefreshToken is required");
    }
}

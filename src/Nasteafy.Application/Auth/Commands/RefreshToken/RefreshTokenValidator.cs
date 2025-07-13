using FluentValidation;
using Nasteafy.Application.Auth.Commands.UseRefreshToken;

namespace Nasteafy.Application.Auth.Refresh
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required");
        }
    }
}

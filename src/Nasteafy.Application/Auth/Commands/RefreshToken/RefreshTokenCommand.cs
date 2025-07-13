using FluentResults;
using MediatR;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Abstractions.Helpers;

namespace Nasteafy.Application.Auth.Commands.UseRefreshToken
{
    public record RefreshTokenCommand(string? RefreshToken) : IRequest<Result<string>>, ITransactionalCommand;

    public class RefreshTokenCommandHandler(
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork,
        IClaimService claimService)
            : IRequestHandler<RefreshTokenCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var user = await unitOfWork.Users.GetByRefreshTokenAsync(request.RefreshToken!, ct);

            if (user is null)
            {
                return Result.Fail("Invalid refresh token").Log<RefreshTokenCommandHandler>();
            }

            if (user.RefreshToken!.IsExpired)
            {
                return Result.Fail("Refresh token expired").Log<RefreshTokenCommandHandler>();
            }

            var claims = await claimService.GenerateClaimsAsync(user, ct);
            var newAccessToken = jwtTokenService.GenerateAccessToken(claims);

            return Result.Ok(newAccessToken);
        }
    }
}
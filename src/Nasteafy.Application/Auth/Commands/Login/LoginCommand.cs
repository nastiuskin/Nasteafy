using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Helpers;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>, ITransactionalCommand;

    public class LoginCommandHandler(
        ISignInService signInService,
        IUserManager userManager,
        IClaimService claimService,
        IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return Result.Fail("Invalid email or password").Log<LoginCommandHandler>();
            }

            var result = await signInService.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Result.Fail("Invalid username or password").Log<LoginCommandHandler>();
            }                

            var roles = await userManager.GetRolesAsync(user);
            var claims = await claimService.GenerateClaimsAsync(user, ct);

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await userManager.UpdateAsync(user);

            return Result.Ok(new AuthResponse(accessToken, refreshToken.Token));
        }
    }
}

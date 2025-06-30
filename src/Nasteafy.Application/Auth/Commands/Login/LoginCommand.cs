using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Users;
using System.Security.Claims;

namespace Nasteafy.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;

    public class LoginCommandHandler(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result.Fail("User not found").Log<LoginCommandHandler>();

            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Result.Fail("Invalid username or password").Log<LoginCommandHandler>();

            var roles = await userManager.GetRolesAsync(user);

            var subscription = await unitOfWork.Subscriptions
                .GetActiveSubscriptionAsync(user.Id, ct);

            var claims = new List<Claim>
            {
                new Claim(ClaimsConstants.UserId, user.Id.ToString()),
                new Claim(ClaimsConstants.Email, user.Email ?? string.Empty),
                new Claim(ClaimsConstants.SubscriptionType, subscription?.Subscription?.Type.Name ?? string.Empty)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsConstants.Role, role));
            }

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await userManager.UpdateAsync(user);

            return Result.Ok(new AuthResponse(accessToken, refreshToken.Token));
        }
    }
}

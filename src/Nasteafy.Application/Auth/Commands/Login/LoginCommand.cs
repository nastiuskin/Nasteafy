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
        // Instead of using SignInManager directly, it is better to wrap them into your interfaces, f.e. ISignInService and IUserManager
        // This way you could mock them easier in Unit Test and you won't depend on a concrete implementation, rather you could swap an identity provider only in two places (ISignInService and IUserManager)
        // instead of changing every command handler and query handler that uses them. This is a part of DIP from solid. 
        // you can also adjust what to return from methods like SignInAsync, wrapping in try catches if needed and returnin specific results that are used in many places.
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            // No need for this check, return same error, like "Invalid username or password". If user is null CheckPasswordSignInAsync should return false.
            // If you return User not found someone can brute force login and check which users exist and which not. 
            /*
                var user = await UserManager.FindByEmailAsync(creds.Email);
                var result = await SignInManager.CheckPasswordSignInAsync(user,
                    creds.Password, true);
                if (result.Succeeded) {
             */

            // You already check that in validator.
            if (user == null)
                return Result.Fail("User not found").Log<LoginCommandHandler>();

            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Result.Fail("Invalid username or password").Log<LoginCommandHandler>();

            var roles = await userManager.GetRolesAsync(user);

            // One method = one line. Two methods = two lines. And so on
            var subscription = await unitOfWork.Subscriptions.GetActiveSubscriptionAsync(user.Id, ct);

            // Repeating code in multiple places, you can move it to IClaimService or IUserManager or IUserService or whatever, especially cause claims should be same in all those places
            var claims = new List<Claim>
            {
                new Claim(ClaimsConstants.UserId, user.Id.ToString()),
                new Claim(ClaimsConstants.Email, user.Email ?? string.Empty), // Can there be a user without Email? 
                new Claim(ClaimsConstants.SubscriptionType, subscription?.Subscription?.Type.Name ?? string.Empty)
            };

            // You can simplify this with one line f.e
            // claims.AddRange(roles.Select(x => new Claim(ClaimsConstants.Role, x)));
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

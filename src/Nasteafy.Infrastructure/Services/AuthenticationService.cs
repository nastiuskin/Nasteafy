using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Auth.Commands.Login;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Constants;
using System.Data;
using System.Security.Claims;

namespace Nasteafy.Infrastructure.Services
{
    public class AuthenticationService(
     SignInManager<User> signInManager,
     UserManager<User> userManager,
     IUnitOfWork unitOfWork,
     IJwtTokenService jwtTokenService,
     IUserIdProvider userProvider) : IAuthenticationService
    {
        public async Task<Result> LogoutAsync()
        {
            var userId = userProvider.GetUserId();
            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Result.Fail("Unauthorized")
                    .LogIfFailed<AuthenticationService>();

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            return Result.Ok();
        }

        public async Task<Result<AuthResponse>> PasswordSignInAsync(string email, string password, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Fail("User not found")
                    .LogIfFailed<AuthenticationService>(); ;

            var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Result.Fail("Invalid username or password")
                    .LogIfFailed<AuthenticationService>(); 

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

        public async Task<Result<string>> RefreshTokenAsync(string refreshToken, CancellationToken ct)
        {
            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken != null && u.RefreshToken.Token == refreshToken);

            if (user is null)
                return Result.Fail("Invalid refresh token")
                    .LogIfFailed<AuthenticationService>(); 

            if (user.RefreshToken!.IsExpired)
                return Result.Fail("Refresh token expired")
                    .LogIfFailed<AuthenticationService>(); 

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

            var newAccessToken = jwtTokenService.GenerateAccessToken(claims);

            return Result.Ok(newAccessToken);
        }

        public async Task<Result<Guid>> RegisterAsync(string email, string password)
        {
            var existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser is not null)
                return Result.Fail("User already exists")
                    .LogIfFailed<AuthenticationService>();

            var user = new User
            {
                Email = email,
                UserName = email,
                Playlists = [],
                UserSubscriptions = []
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return Result.Fail(string.Join(", ", result.Errors.Select(e => e.Description)))
                    .LogIfFailed<AuthenticationService>();

            var roleAssignResult = await userManager.AddToRoleAsync(user, "User");
            if (!roleAssignResult.Succeeded)
                return Result.Fail(string.Join(", ", roleAssignResult.Errors.Select(e => e.Description)))
                    .LogIfFailed<AuthenticationService>();

            return Result.Ok(user.Id);
        }
    }
}
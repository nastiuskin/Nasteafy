using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Constants;
using System.Data;
using System.Security.Claims;

namespace Nasteafy.Infrastructure.Services
{
    public class AuthenticationService(
     SignInManager<User> signInManager,
     UserManager<User> userManager,
     IJwtTokenService jwtTokenService,
     IHttpContextAccessor httpContextAccessor,
     IUserIdProvider userProvider) : IAuthenticationService
    {
        public async Task<Result> LogoutAsync()
        {
            var userId = userProvider.GetUserId();
            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Result.Fail("Unauthorized");

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");

            return Result.Ok();
        }

        public async Task<AuthResult> PasswordSignInAsync(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return AuthResult.Failure("User not found");

            var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return AuthResult.Failure("Invalid username or password");

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimsConstants.UserId, user.Id.ToString()),
                new Claim(ClaimsConstants.Email, user.Email ?? string.Empty),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsConstants.Role, role));
            }

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await userManager.UpdateAsync(user);

            return AuthResult.Success(accessToken, refreshToken.Token);
        }

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken != null && u.RefreshToken.Token == refreshToken);

            if (user is null)
                return AuthResult.Failure("Invalid refresh token");

            if (user.RefreshToken!.IsExpired)
                return AuthResult.Failure("Refresh token expired");

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimsConstants.UserId, user.Id.ToString()),
                new Claim(ClaimsConstants.Email, user.Email ?? string.Empty),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsConstants.Role, role));
            }

            var newAccessToken = jwtTokenService.GenerateAccessToken(claims);

            await userManager.UpdateAsync(user);

            return AuthResult.Success(newAccessToken, refreshToken);
        }

        public async Task<Result> RegisterAsync(string userName, string password)
        {
            var existingUser = await userManager.FindByNameAsync(userName);
            if (existingUser is not null)
                return Result.Fail("User already exists");

            var user = new User
            {
                Email = userName,
                UserName = userName,
                Playlists = [],
                UserSubscriptions = []
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return Result.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            var roleAssignResult = await userManager.AddToRoleAsync(user, "User");
            if (!roleAssignResult.Succeeded)
                return Result.Fail(string.Join(", ", roleAssignResult.Errors.Select(e => e.Description)));

            return Result.Ok();
        }
    }
}
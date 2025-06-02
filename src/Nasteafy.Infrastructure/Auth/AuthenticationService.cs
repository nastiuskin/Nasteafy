using Microsoft.AspNetCore.Identity;
using Nasteafy.Application.Abstractions.Auth;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Constants;
using System.Security.Claims;

namespace Nasteafy.Infrastructure.Auth
{
    public class AuthenticationService(
         SignInManager<User> signInManager,
         UserManager<User> userManager,
         IJwtTokenService jwtTokenService) : IAuthenticationService
    {
        public async Task<AuthResult> PasswordSignInAsync(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return AuthResult.Failure("User not found");

            var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return AuthResult.Failure("Invalid username or password");

            var claims = new List<Claim>
            {
                new Claim(ClaimsConstants.UserId, user.Id.ToString()),
                new Claim(ClaimsConstants.Email, user.Email ?? string.Empty),
            };

            var accessToken = jwtTokenService.GenerateAccessToken(claims);
            var refreshToken = jwtTokenService.GenerateRefreshToken();

            return AuthResult.Success(accessToken, refreshToken);
        }

        public async Task<AuthResult> RegisterAsync(string userName, string password)
        {
            var existingUser = await userManager.FindByNameAsync(userName);
            if (existingUser is not null)
                return AuthResult.Failure("User already exists");

            var user = new User
            {
                Email = userName,
                UserName = userName,
                Playlists = [],
                UserSubscriptions = []
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return AuthResult.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

            return await PasswordSignInAsync(userName, password);
        }
    }
}
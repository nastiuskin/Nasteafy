using FluentResults;

namespace Nasteafy.Application.Abstractions.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthResult> PasswordSignInAsync(string userName, string password);
        Task<Result> RegisterAsync(string userName, string password);
        Task<Result> LogoutAsync();
        Task<AuthResult> RefreshTokenAsync(string refreshToken);
    }
}

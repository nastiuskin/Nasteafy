using FluentResults;
using Nasteafy.Application.Auth.Commands.Login;

namespace Nasteafy.Application.Common.Abstractions.Auth
{
    public interface IAuthenticationService
    {
        Task<Result<AuthResponse>> PasswordSignInAsync(string userName, string password);
        Task<Result> RegisterAsync(string userName, string password);
        Task<Result> LogoutAsync();
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
    }
}

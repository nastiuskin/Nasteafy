using FluentResults;

namespace Nasteafy.Application.Abstractions.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthResult> PasswordSignInAsync(string userName, string password);
        Task<AuthResult> RegisterAsync(string userName, string password);
    }
}

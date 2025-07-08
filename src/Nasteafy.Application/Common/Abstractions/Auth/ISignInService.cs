using Microsoft.AspNetCore.Identity;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Common.Abstractions.Auth
{
    public interface ISignInService
    {
        Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure);
    }
}

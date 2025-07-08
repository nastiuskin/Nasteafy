using Microsoft.AspNetCore.Identity;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Infrastructure.Services
{
    public class SignInService(SignInManager<User> signInManager) : ISignInService
    {
        public Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
        {
            return signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }
    }
}
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Nasteafy.Application.Common.Abstractions.Auth;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Infrastructure.Services
{
    public class UserManager(UserManager<User> userManager) : IUserManager
    {
        public Task<User?> FindByEmailAsync(string email) => userManager.FindByEmailAsync(email);

        public Task<IList<string>> GetRolesAsync(User user) => userManager.GetRolesAsync(user);

        public Task UpdateAsync(User user) => userManager.UpdateAsync(user);

        public async Task<Result> CreateAsync(User user, string password)
        {
            var identityResult = await userManager.CreateAsync(user, password);
            return identityResult.Succeeded
                ? Result.Ok()
                : Result.Fail(identityResult.Errors.Select(e => new Error(e.Description)));
        }

        public async Task<Result> AddToRoleAsync(User user, string role)
        {
            var identityResult = await userManager.AddToRoleAsync(user, role);
            return identityResult.Succeeded
                ? Result.Ok()
                : Result.Fail(identityResult.Errors.Select(e => new Error(e.Description)));
        }   
        
        public async Task<Result> RemoveRoleAsync(User user, string role)
        {
            var identityResult = await userManager.RemoveFromRoleAsync(user, role);
            return identityResult.Succeeded
                ? Result.Ok()
                : Result.Fail(identityResult.Errors.Select(e => new Error(e.Description)));
        }
    }
}
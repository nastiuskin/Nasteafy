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
    }
}
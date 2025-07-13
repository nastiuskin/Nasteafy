using FluentResults;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Common.Abstractions.Auth
{
    public interface IUserManager
    {
        Task<User?> FindByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(User user);
        Task UpdateAsync(User user);
        Task<Result> CreateAsync(User user, string password);
        Task<Result> AddToRoleAsync(User user, string role);
        Task<Result> RemoveRoleAsync(User user, string role);
    }
}

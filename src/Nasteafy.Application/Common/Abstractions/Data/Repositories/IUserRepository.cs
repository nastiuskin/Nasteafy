using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Common.Abstractions.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetWithSubscriptionsAsync(Guid userId, CancellationToken ct);
    }
}

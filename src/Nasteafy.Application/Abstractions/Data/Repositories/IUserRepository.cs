using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Abstractions
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetWithSubscriptionsAsync(Guid userId, CancellationToken ct);
    }
}

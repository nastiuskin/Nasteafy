using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Application.Common.Abstractions.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByIdWithSubscriptionsAsync(Guid userId, CancellationToken ct);
    }
}

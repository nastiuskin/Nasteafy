using Nasteafy.Domain.Entities;
using Nasteafy.Domain.Enums;

namespace Nasteafy.Application.Abstractions
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetPremiumUsersAsync(CancellationToken ct);
        Task<bool> IsSubscriptionActiveAsync(Guid userId, CancellationToken ct);
    }
}

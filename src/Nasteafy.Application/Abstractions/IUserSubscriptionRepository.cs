using Nasteafy.Domain.Entities;

namespace Nasteafy.Application.Abstractions
{
    public interface IUserSubscriptionRepository : IBaseRepository<UserSubscription>
    {
        Task<UserSubscription?> GetActiveSubscriptionAsync(Guid userId, CancellationToken ct);        
    }
}

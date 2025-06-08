using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Application.Abstractions.Data.Repositories
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType, CancellationToken ct);
        Task<UserSubscription?> GetActiveSubscriptionAsync(Guid userId, CancellationToken ct);
        Task CancelActiveSubscriptionAsync(Guid userId, CancellationToken ct);
    }
}

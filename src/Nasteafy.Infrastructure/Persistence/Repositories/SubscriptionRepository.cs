using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions.Data.Repositories;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Infrastructure.Persistence.Contexts;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(DatabaseContext context) : base(context) { }

        public async Task<UserSubscription?> GetActiveSubscriptionAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Set<UserSubscription>()
                .FirstOrDefaultAsync(us => us.UserId == userId && us.EndDate > DateTime.UtcNow, ct);
        }

        public async Task CancelActiveSubscriptionAsync(Guid userId, CancellationToken ct)
        {
            var subscription = await GetActiveSubscriptionAsync(userId, ct);
            if (subscription is null)
                throw new InvalidOperationException("No active subscription to cancel.");

            subscription.EndDate = DateTime.UtcNow;
        }

        public async Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType, CancellationToken ct)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Type == SubscriptionType.Free);
        }
    }
}

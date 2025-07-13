using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
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
                .Include(x => x.Subscription)
                .FirstOrDefaultAsync(us => us.UserId == userId && us.EndDate > DateTime.UtcNow, ct);
        }

        public async Task<Subscription?> GetByTypeAsync(SubscriptionType subscriptionType, CancellationToken ct)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Type == SubscriptionType.Free, ct);
        }

        public async Task<List<UserSubscription>> GetAllByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Set<UserSubscription>()
                .Where(x => x.UserId == userId)
                .Include(x => x.Subscription)
                .ToListAsync(ct);
        }
    }
}

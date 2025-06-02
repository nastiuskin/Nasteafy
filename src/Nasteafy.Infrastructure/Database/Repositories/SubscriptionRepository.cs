using Nasteafy.Application.Abstractions.Data.Repositories;
using Nasteafy.Domain.Entities.Subscriptions;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(DatabaseContext context) : base(context) { }

    }
}

using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Domain.Entities.Subscriptions
{
    public class UserSubscription : BaseEntity
    { 
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }

        public required Subscription Subscription { get; set; }
        public required Guid SubscriptionId { get; set; }

        public required User User { get; set; }
        public required Guid UserId { get; set; }
    }
}

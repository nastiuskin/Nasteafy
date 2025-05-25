using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Domain.Entities.Subscriptions
{
    public class UserSubscription
    { 
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }

        public Subscription Subscription { get; set; }
        public required Guid SubscriptionId { get; set; }

        public User User { get; set; }
        public required Guid UserId { get; set; }
    }
}

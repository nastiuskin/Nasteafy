using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities.Subscriptions
{
    public class Subscription : BaseEntity
    {
        public required SubscriptionType Type { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }

        public required virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}

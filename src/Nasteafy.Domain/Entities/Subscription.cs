using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Enums;

namespace Nasteafy.Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public required SubscriptionType Type { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }

        public ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}

using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities
{
    public class UserSubscription : BaseEntity
    {
        public required DateTime StartDate { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public required Guid SubscriptionId { get; set; }
        public required Guid UserId { get; set; }

        public required User User { get; set; }
        public required Subscription Subscription { get; set; }
    }
}

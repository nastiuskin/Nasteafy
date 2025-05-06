using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public Guid UserSubscriptionId { get; set; }

        public UserSubscription UserSubscription { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
    }
}

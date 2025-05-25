using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        public required virtual ICollection<Playlist> Playlists { get; set; }
        public required virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}

using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Entities.Songs;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public Artist? Artist { get; set; }
        public Guid? ArtistId { get; set; }

        public UserSubscription? UserSubscription { get; set; }
        public Guid? UserSubscriptionId { get; set; }

        public required virtual ICollection<Playlist> Playlists { get; set; }
    }
}

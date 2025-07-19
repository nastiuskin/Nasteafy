using Microsoft.AspNetCore.Identity;
using Nasteafy.Domain.Base;
using Nasteafy.Domain.Entities.Subscriptions;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Domain.Entities.Users
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public string? AvatarUrl { get; set; }
        public RefreshToken? RefreshToken { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; } = [];
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = [];
        public virtual ICollection<AlbumRating> AlbumRatings { get; set; } = [];
        public virtual ICollection<TrackLike> TrackLikes { get; set; } = [];
    }
}

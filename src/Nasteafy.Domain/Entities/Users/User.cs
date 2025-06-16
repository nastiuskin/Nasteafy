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

        public required virtual ICollection<Playlist> Playlists { get; set; }
        public required virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}

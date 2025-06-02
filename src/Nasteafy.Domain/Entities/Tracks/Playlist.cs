using Nasteafy.Domain.Base;
using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class Playlist : BaseEntity
    {
        public required string Title { get; set; }

        public User User { get; set; }
        public required Guid UserId { get; set; }

        public required virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}


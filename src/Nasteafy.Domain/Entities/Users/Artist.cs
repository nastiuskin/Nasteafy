using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class Artist : BaseEntity
    {
        public required string Name { get; set; }

        public required User User { get; set; }
        public required Guid UserId { get; set; }

        public required virtual ICollection<AlbumArtist> AlbumArtists { get; set; }
        public required virtual ICollection<ArtistTrack> ArtistTracks { get; set; }
    }
}

using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class Artist : BaseEntity
    {
        public required string Name { get; set; }
        public string? AvatarUrl { get; set; }
        public bool CreatedByAdmin { get; set; }

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public required virtual ICollection<AlbumArtist> AlbumArtists { get; set; }
        public required virtual ICollection<ArtistTrack> ArtistTracks { get; set; }
    }
}

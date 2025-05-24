using Nasteafy.Domain.Contracts;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class Track : BaseEntity
    {
        public required string Title { get; set; }
        public required TimeSpan Duration { get; set; }
        public required string FilePath { get; set; }

        public Guid? AlbumId { get; set; }
        public Album? Album { get; set; }

        public required virtual ICollection<ArtistTrack> ArtistTracks { get; set; }
        public required virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}

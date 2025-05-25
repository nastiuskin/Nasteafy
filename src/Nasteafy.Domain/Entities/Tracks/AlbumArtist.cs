using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class AlbumArtist 
    {
        public Album Album { get; set; }
        public required Guid AlbumId { get; set; }

        public required Guid ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}

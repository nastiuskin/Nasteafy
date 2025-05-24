using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class AlbumArtist : BaseEntity
    {
        public required Album Album { get; set; }
        public required Guid AlbumId { get; set; }

        public required Guid ArtistId { get; set; }
        public required Artist Artist { get; set; }
    }
}

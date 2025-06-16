using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities.Tracks
{
    public class Album : BaseEntity
    {
        public required DateTime ReleaseDate { get; set; }
        public required string Title { get; set; }
        public string? CoverUrl { get; set; }

        public required virtual ICollection<AlbumArtist> AlbumArtists { get; set; }
        public required virtual ICollection<Track> Tracks { get; set; }
    }
}

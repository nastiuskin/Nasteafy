using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities
{
    public class Track : MediaEntity
    {
        public required TimeSpan Duration { get; set; }
        public required Guid ArtistId { get; set; }
        public float Rating { get; set; }
        public Guid? AlbumId {  get; set; }
        public string? YouTubeUrl { get; set; }

        public required Artist Artist { get; set; }
        public Album? Album { get; set; }

        public ICollection<Genre> Genres { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
    }
}

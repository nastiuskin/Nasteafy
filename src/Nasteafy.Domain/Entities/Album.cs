using Nasteafy.Domain.Contracts;

namespace Nasteafy.Domain.Entities
{
    public class Album : MediaEntity
    {
        public required Guid ArtistId { get; set; }

        public required Artist Artist { get; set; } //either band or solo artist
        public ICollection<Track> Tracks { get; set; }
    }
}

using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Domain.Entities
{
    public class ArtistTrack
    {
        public required Track Track { get; set; }
        public required Guid TrackId { get; set; }

        public required Artist Artist { get; set; }
        public required Guid ArtistId { get; set; }
    }   
}

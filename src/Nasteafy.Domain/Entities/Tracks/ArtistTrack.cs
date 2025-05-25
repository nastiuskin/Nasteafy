using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Domain.Entities
{
    public class ArtistTrack
    {
        public Track Track { get; set; }
        public required Guid TrackId { get; set; }

        public Artist Artist { get; set; }
        public required Guid ArtistId { get; set; }
    }   
}

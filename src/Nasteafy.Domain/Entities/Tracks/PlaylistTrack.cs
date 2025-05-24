namespace Nasteafy.Domain.Entities.Tracks
{
    public class PlaylistTrack
    {
        public required int Order { get; set; }

        public required Track Track { get; set; }
        public required Guid TrackId { get; set; }

        public required Playlist Playlist { get; set; }
        public required Guid PlaylistId { get; set; }
    }   
}

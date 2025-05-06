namespace Nasteafy.Shared.Dtos.Tracks
{
    public class GetTrackDto
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required TimeSpan Duration { get; set; }
        public required string ArtistName { get; set; }
    }
}

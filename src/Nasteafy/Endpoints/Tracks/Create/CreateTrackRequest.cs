namespace Nasteafy.Endpoints.Tracks.Create
{
    public sealed record CreateTrackRequest
    {
        public required string Title { get; init; }
        public required IFormFile File { get; init; }
        public required TimeSpan Duration { get; init; }
        public Guid? AlbumId { get; init; }
        public List<Guid> Artists { get; init; } = [];
    }
}

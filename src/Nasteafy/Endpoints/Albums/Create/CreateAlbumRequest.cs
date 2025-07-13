namespace Nasteafy.Endpoints.Albums.Create
{
    public sealed record CreateAlbumRequest
    {
        public required string Title { get; init; }
        public IFormFile? CoverFile { get; init; }
        public required DateTime ReleaseDate { get; init; }
        public List<Guid> Artists { get; init; } = [];
    }
}

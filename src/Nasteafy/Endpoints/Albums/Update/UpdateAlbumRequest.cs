namespace Nasteafy.Endpoints.Albums.Update
{
    public sealed record UpdateAlbumRequest
    {
        public IFormFile? CoverFile { get; init; }
        public DateTime ReleaseDate { get; init; }
        public string? Title { get; init; }
    }
}

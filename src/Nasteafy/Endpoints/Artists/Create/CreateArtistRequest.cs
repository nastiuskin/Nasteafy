namespace Nasteafy.Endpoints.Artists.Create
{
    public sealed record CreateArtistRequest
    {
        public required string Name { get; init; }
        public string? Biography { get; init; }
        public IFormFile? ArtistPhoto { get; init; }
    }
}

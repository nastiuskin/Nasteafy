namespace Nasteafy.Endpoints.Artists.Update
{
    public sealed record UpdateArtistRequest
    {
        public string? Name { get; init; }
        public IFormFile? AvatarFile { get; init; }
    }
}


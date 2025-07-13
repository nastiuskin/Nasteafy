namespace Nasteafy.Endpoints.Playlists.Create
{
    public class CreatePlaylistRequest
    {
        public required string Title { get; set; }
        public IFormFile? PlaylistCover { get; set; }
    }
}

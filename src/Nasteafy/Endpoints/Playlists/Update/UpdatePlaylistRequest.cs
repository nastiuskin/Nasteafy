namespace Nasteafy.Endpoints.Playlists.Update
{
    public class UpdatePlaylistRequest
    {
        public string? Title { get; set; }
        public IFormFile? CoverFile { get; set; }
    }
}

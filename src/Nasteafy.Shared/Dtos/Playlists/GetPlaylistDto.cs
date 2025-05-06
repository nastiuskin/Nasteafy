namespace Nasteafy.Shared.Dtos.Playlists
{
    public class GetPlaylistDto
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}

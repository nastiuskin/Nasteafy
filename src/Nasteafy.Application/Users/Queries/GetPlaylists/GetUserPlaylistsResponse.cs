using Nasteafy.Application.Playlists.Queries.GetById;

namespace Nasteafy.Application.Users.Queries.GetPlaylists
{
    public class GetUserPlaylistsResponse
    {
        public List<PlaylistDetailsDto> Playlists { get; set; } = [];
    }
}


namespace Nasteafy.Application.Playlists.Queries.GetByUserId
{
    public record GetUserPlaylistsResponse(List<UserPlaylistDto> Playlists);

    public record UserPlaylistDto(Guid Id,
        string Title,
        string? CoverUrl,
        int TracksCount);
}


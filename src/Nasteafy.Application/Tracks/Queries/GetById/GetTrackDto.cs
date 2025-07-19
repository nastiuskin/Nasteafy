namespace Nasteafy.Application.Tracks.Queries.GetById
{
    public record GetTrackDto(Guid Id,
        string Title, 
        string ArtistName, 
        string PathUrl,
        TimeSpan Duration,
        string? AlbumCover,
        bool IsLiked);
}

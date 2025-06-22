namespace Nasteafy.Application.Albums.Queries.GetById
{
    public record AlbumDto(Guid Id,
        string Title,
        string? CoverUrl,
        string Artist);
}

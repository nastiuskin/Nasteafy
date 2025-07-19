namespace Nasteafy.Application.Albums.Queries.GetById
{
    public record AlbumDto(Guid Id,
        string Title,
        DateTime ReleaseDate,
        string? CoverUrl,
        string Artist,
        double AverageRating,
        int? UserRating);
}

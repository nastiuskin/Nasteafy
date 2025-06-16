namespace Nasteafy.Application.Artists.Queries.GetAll
{
    public record GetAllArtistsResponse(List<ArtistDto> Artists);
    public record ArtistDto(Guid Id, string? AvatarUrl, string Name);
}

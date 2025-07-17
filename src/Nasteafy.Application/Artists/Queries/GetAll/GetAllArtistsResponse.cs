namespace Nasteafy.Application.Artists.Queries.GetAll
{
    public record ArtistDto(
        Guid Id,
        string? AvatarUrl, 
        string Name, 
        string? Biography,
        bool IsVerified);
}

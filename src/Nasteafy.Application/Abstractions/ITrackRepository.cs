using Nasteafy.Domain.Entities;

namespace Nasteafy.Application.Abstractions
{
    public interface ITrackRepository : IBaseRepository<Track>
    {
        Task<IEnumerable<Track>> GetByArtistIdAsync(Guid artistId, CancellationToken ct);
        Task<IEnumerable<Track>> GetByAlbumIdAsync(Guid albumId, CancellationToken ct);
        Task<IEnumerable<Track>> GetByPlaylistIdAsync(Guid playlistId, CancellationToken ct);
        Task<IEnumerable<Track>> GetByGenreAsync(Guid genreId, CancellationToken ct);
        Task<IEnumerable<Track>> GetByNameAsync(string name, CancellationToken ct);
    }
}

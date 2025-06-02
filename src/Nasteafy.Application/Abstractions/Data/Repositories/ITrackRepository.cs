using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Abstractions
{
    public interface ITrackRepository : IGenericRepository<Track>
    {
        Task<IEnumerable<Track>> GetByArtistId(Guid artistId, CancellationToken ct);
        Task<IEnumerable<Track>> GetByAlbumId(Guid albumId, CancellationToken ct);
        Task<IEnumerable<Track>> GetByPlaylistId(Guid playlistId, CancellationToken ct);
        Task<IEnumerable<Track>> GetByName(string name, CancellationToken ct);
    }
}

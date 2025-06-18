using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Common.Abstractions.Data.Repositories
{
    public interface ITrackRepository : IGenericRepository<Track>
    {
        Task<PagedResult<Track>> GetByArtistIdAsync(Guid artistId,PagedRequest req, CancellationToken ct);
        Task<PagedResult<Track>> GetByAlbumIdAsync(Guid albumId, PagedRequest req, CancellationToken ct);
        Task<PagedResult<Track>> GetByPlaylistIdAsync(Guid playlistId, PagedRequest req, CancellationToken ct);
        Task<PagedResult<Track>> GetByTitleAsync(string title, PagedRequest req, CancellationToken ct);
        //IQueryable<Track> GetByIdWithArtists(Guid id);
    }
}

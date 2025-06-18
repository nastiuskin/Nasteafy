using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Common.Abstractions.Data.Repositories
{
    public interface IPlaylistRepository : IGenericRepository<Playlist>
    {
        //IQueryable<Playlist> GetByUserIdWithTracks(Guid userId, PagedRequest request, CancellationToken ct); //TO DELETE

        Task<PagedResult<Playlist>> GetByUserIdAsync(Guid userId, PagedRequest request, CancellationToken ct);
        Task<Playlist?> GetByIdWithTracks(Guid id, CancellationToken ct);
        Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct);
        Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct);
    }
}

using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Abstractions
{
    public interface IPlaylistRepository : IGenericRepository<Playlist>
    {
        Task<IEnumerable<Playlist>> GetByUserId(Guid userId, CancellationToken ct);
        Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct);
        Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct);
    }
}

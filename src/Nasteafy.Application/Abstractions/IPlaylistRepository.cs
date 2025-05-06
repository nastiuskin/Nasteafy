using Nasteafy.Domain.Entities;

namespace Nasteafy.Application.Abstractions
{
    public interface IPlaylistRepository : IBaseRepository<Playlist>
    {
        Task<IEnumerable<Playlist>> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct);
        Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct);
        Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync(CancellationToken ct);
    }
}

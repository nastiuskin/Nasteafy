using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Abstractions
{
    public interface IPlaylistRepository : IGenericRepository<Playlist>
    {
        IQueryable<Playlist> GetByUserIdWithTracks(Guid userId, CancellationToken ct);
        IQueryable<Playlist> GetByIdWithTracks(Guid id, CancellationToken ct);
        Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct);
        Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct);
    }
}

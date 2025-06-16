using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Infrastructure.Persistence.Contexts;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class PlaylistRepository : GenericRepository<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(DatabaseContext context) : base(context) { }

        public async Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct)
        {
            var currentMaxOrder = await _context.PlaylistTracks
             .Where(pt => pt.PlaylistId == playlistId)
             .MaxAsync(pt => pt.Order, ct);

            var entity = new PlaylistTrack
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                Order = currentMaxOrder + 1
            };

            await _context.PlaylistTracks.AddAsync(entity, ct);
        }

        public IQueryable<Playlist> GetByIdWithTracks(Guid id, CancellationToken ct)
        {
            return _context.Playlists
                .Where(x => x.Id == id);
        }   

        public IQueryable<Playlist> GetByUserIdWithTracks(Guid userId, CancellationToken ct)
        {
            return _context.Playlists
                .Where(p => p.UserId == userId);
        }

        public async Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct)
        {
            var entity = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId, ct);

            if (entity != null)
            {
                _context.PlaylistTracks.Remove(entity);

                var remainingTracks = await _context.PlaylistTracks
                    .Where(pt => pt.PlaylistId == playlistId && pt.Order > entity.Order)
                    .ToListAsync(ct);

                foreach (var track in remainingTracks)
                {
                    track.Order--;
                }
            }
        }
    }
}

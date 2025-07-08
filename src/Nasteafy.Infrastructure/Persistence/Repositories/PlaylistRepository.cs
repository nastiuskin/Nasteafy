using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Infrastructure.Persistence.Contexts;
using Nasteafy.Infrastructure.Persistence.Extensions;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class PlaylistRepository : GenericRepository<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(DatabaseContext context) : base(context) { }

        public async Task AddTrackToPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct)
        {
            var currentMaxOrder = await _context.PlaylistTracks
                .Where(pt => pt.PlaylistId == playlistId)
                .Select(pt => pt.Order)
                .MaxAsync(ct);

            var entity = new PlaylistTrack
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                Order = currentMaxOrder + 1
            };

            await _context.PlaylistTracks.AddAsync(entity, ct);
        }

        public async Task<Playlist?> GetByIdWithTracks(Guid id, CancellationToken ct)
        {
            return await _context.Playlists
              .Where(x => x.Id == id)
              .Include(p => p.PlaylistTracks)
              .FirstOrDefaultAsync(ct);
        }

        public async Task<PagedResult<Playlist>> GetByUserIdAsync(Guid userId, PagedRequest request, CancellationToken ct)
        {
            var query = _context.Playlists
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.PlaylistTracks);

            return await query.ToPagedResultAsync(request, ct);
        }

        public IQueryable<Playlist> GetByUserIdWithTracks(Guid userId, PagedRequest request, CancellationToken ct)
        {
            return _context.Playlists
                .Where(p => p.UserId == userId)
                .Include(p => p.PlaylistTracks)
                    .ThenInclude(pt => pt.Track);
        }

        public async Task RemoveTrackFromPlaylistAsync(Guid playlistId, Guid trackId, CancellationToken ct)
        {
            var playlistTracks = await _context.PlaylistTracks
                .Where(pt => pt.PlaylistId == playlistId)
                .ToListAsync(ct);

            var playlistTrackToRemove = playlistTracks.FirstOrDefault(x => x.TrackId == trackId);

            if (playlistTrackToRemove is not null)
            {
                _context.PlaylistTracks.Remove(playlistTrackToRemove);

                foreach (var track in playlistTracks.Where(x => x.Order > playlistTrackToRemove.Order))
                {
                    track.Order--;
                }
            }
        }
    }
}

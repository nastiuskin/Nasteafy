using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Infrastructure.Persistence.Contexts;
using Nasteafy.Infrastructure.Persistence.Extensions;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class TrackRepository : GenericRepository<Track>, ITrackRepository
    {
        public TrackRepository(DatabaseContext context) : base(context) { }

        public async Task<PagedResult<Track>> GetByAlbumIdAsync(Guid albumId, PagedRequest req, CancellationToken ct)
        {
            var query = _context.Tracks
                .AsNoTracking()
                .Where(x => x.AlbumId == albumId)
                .Include(x => x.Album);

            return await query.ToPagedResultAsync(req, ct);
        }

        public async Task<PagedResult<Track>> GetByArtistIdAsync(Guid artistId, PagedRequest req, CancellationToken ct)
        {
            var query = _context.ArtistTracks
                .AsNoTracking()
                .Where(x => x.ArtistId == artistId)
                    .Include(x => x.Artist)
                .Select(x => x.Track);

            return await query.ToPagedResultAsync(req, ct);
        }

        public async Task<PagedResult<Track>> GetByTitleAsync(string title, PagedRequest req, CancellationToken ct)
        {
            var query = _context.Tracks
                .AsNoTracking()
                .Where(x => x.Title == title);

            return await query.ToPagedResultAsync(req, ct);
        }

        public async Task<PagedResult<Track>> GetByPlaylistIdAsync(Guid playlistId, PagedRequest req, CancellationToken ct)
        {
            var query = _context.PlaylistTracks
             .AsNoTracking()
             .Where(x => x.PlaylistId == playlistId)
             .Include(x => x.Track)                            
                 .ThenInclude(t => t.ArtistTracks)             
                     .ThenInclude(at => at.Artist)  
              .Include(x => x.Track)
                .ThenInclude(x => x.Album)
             .Select(x => x.Track);

            return await query.ToPagedResultAsync(req, ct);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Infrastructure.Persistence.Contexts;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class TrackRepository : GenericRepository<Track>, ITrackRepository
    {
        public TrackRepository(DatabaseContext context) : base(context) { }

        public async Task<IEnumerable<Track>> GetByAlbumId(Guid albumId, CancellationToken ct)
        {
           return await _context.Tracks
                .Where(x => x.AlbumId == albumId)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Track>> GetByArtistId(Guid artistId, CancellationToken ct)
        {
            return await _context.ArtistTracks
                .Where(x => x.ArtistId  == artistId)
                .Select(x => x.Track)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Track>> GetByName(string name, CancellationToken ct)
        {
           return await _context.Tracks
                .Where(x => x.Title.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Track>> GetByPlaylistId(Guid playlistId, CancellationToken ct)
        {
            return await _context.PlaylistTracks
                .Where(x => x.PlaylistId == playlistId)
                .Select(x => x.Track)
                .ToListAsync (ct);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Infrastructure.Database.Repositories;
using Nasteafy.Infrastructure.Persistence.Contexts;
using Nasteafy.Infrastructure.Persistence.Extensions;

namespace Nasteafy.Infrastructure.Persistence.Repositories
{
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(DatabaseContext context) : base(context) { }

        public async Task<PagedResult<Album>> GetAlbumsByArtistIdAsync(Guid artistId, PagedRequest request, CancellationToken ct)
        {
            var query = _context.Albums
                .AsNoTracking()
                .Where(album => album.AlbumArtists.Any(aa => aa.ArtistId == artistId))
                .Include(album => album.AlbumArtists)
                    .ThenInclude(aa => aa.Artist)
                .Include(x => x.Ratings);

            return await query.ToPagedResultAsync(request, ct);
        }

        public async Task<PagedResult<Album>> GetAllAlbumsWithRatings(PagedRequest request, CancellationToken ct)
        {
            var query = _context.Albums
                .Include(a => a.Ratings);

            return await query.ToPagedResultAsync(request, ct);
        }
    }
}

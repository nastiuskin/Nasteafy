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

        public async Task<PagedResult<Album>> GetByArtistIdAsync(Guid artistId, PagedRequest request, CancellationToken ct)
        {
            var query = _context.AlbumArtists
              .AsNoTracking()
              .Where(x => x.ArtistId == artistId)
                .Include(x => x.Artist)
               .Select(x => x.Album);

            return await query.ToPagedResultAsync(request, ct);
        }
    }
}

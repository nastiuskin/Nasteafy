using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;
using Nasteafy.Infrastructure.Persistence.Contexts;
using Nasteafy.Infrastructure.Persistence.Extensions;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(DatabaseContext context) : base(context) { }

        public async Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Artists.AnyAsync(x => x.UserId == userId, ct);
        }

        public IQueryable<Artist> FindAllByIds(List<Guid> ids)
        {
            return _context.Artists
             .AsNoTracking()
             .Where(a => ids.Contains(a.Id));
        }

        public async Task<PagedResult<Artist>> GetAllArtists(PagedRequest request, CancellationToken ct)
        {
            var query = _context.Artists.AsNoTracking();

            return await query.ToPagedResultAsync(request, ct);
        }

        public async Task<PagedResult<Artist>> GetByNameAsync(string name, PagedRequest request, CancellationToken ct)
        {
            var query = _context.Artists
                .AsNoTracking()
                .Where(x => x.Name.ToLower().Contains(name.ToLower()));

            return await query.ToPagedResultAsync(request, ct);
        }
    }
}

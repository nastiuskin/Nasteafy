using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions.Data.Repositories;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(DatabaseContext context) : base(context) { }

        public async Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.Artists
                 .AnyAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Artist>> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Artists
                .Where(x => x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync(ct);
        }
    }
}

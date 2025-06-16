using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Persistence.Contexts;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context) { }

        public async Task<User?> GetWithSubscriptionsAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.UserSubscriptions)
                .ThenInclude(x => x.Subscription)
                .FirstOrDefaultAsync(u => u.Id == userId, ct);
        }
    }
}

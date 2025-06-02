using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context) { }

        public async Task<User?> GetWithSubscriptionsAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Users
                .Include(u => u.UserSubscriptions)
                .FirstOrDefaultAsync(u => u.Id == userId, ct);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Domain.Entities.Users;
using Nasteafy.Infrastructure.Persistence.Contexts;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context) { }

        public async Task<User?> GetByIdWithSubscriptionsAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UserSubscriptions)
                    .ThenInclude(x => x.Subscription)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct)
        {
            return await _context.Users                
                .Where(u => u.RefreshToken != null && u.RefreshToken.Token == refreshToken)
                .FirstOrDefaultAsync(ct);
        }
    }
}

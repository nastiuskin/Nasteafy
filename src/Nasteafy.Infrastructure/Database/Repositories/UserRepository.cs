using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Entities.Users;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context) { }
    }
}

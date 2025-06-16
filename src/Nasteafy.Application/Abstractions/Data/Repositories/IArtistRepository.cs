using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Abstractions.Data.Repositories
{
    public interface IArtistRepository : IGenericRepository<Artist>
    {
        Task<IEnumerable<Artist>> GetByNameAsync(string name, CancellationToken ct);
        Task<IEnumerable<Artist>> GetAllIncludeUsers(CancellationToken ct);
        Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct);
    }
}

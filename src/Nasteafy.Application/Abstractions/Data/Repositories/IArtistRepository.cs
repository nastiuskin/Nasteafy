using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Abstractions.Data.Repositories
{
    public interface IArtistRepository : IBaseRepository<Artist>
    {
        Task<IEnumerable<Artist>> GetByNameAsync(string name, CancellationToken ct);
        Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct);
    }
}

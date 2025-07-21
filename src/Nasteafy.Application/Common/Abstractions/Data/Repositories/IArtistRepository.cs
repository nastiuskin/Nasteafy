using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Common.Abstractions.Data.Repositories
{
    public interface IArtistRepository : IGenericRepository<Artist>
    {
        Task<PagedResult<Artist>> GetByNameAsync(string name, PagedRequest request, CancellationToken ct);
        IQueryable<Artist> FindAllByIds(List<Guid> ids);
        Task<Artist?> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct);
    }
}


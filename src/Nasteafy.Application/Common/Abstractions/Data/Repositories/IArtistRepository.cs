using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Entities.Tracks;

namespace Nasteafy.Application.Common.Abstractions.Data.Repositories
{
    public interface IArtistRepository : IGenericRepository<Artist>
    {
        Task<PagedResult<Artist>> GetByNameAsync(string name, PagedRequest request, CancellationToken ct);
        Task<PagedResult<Artist>> GetAllArtistsIncludeUsers(PagedRequest request, CancellationToken ct);
        IQueryable<Artist> FindAllByIds(List<Guid> ids);
        Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken ct);
    }
}


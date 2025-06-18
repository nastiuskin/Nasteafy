using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Base;
using Nasteafy.Domain.Contracts;
using System.Linq.Expressions;

namespace Nasteafy.Application.Common.Abstractions.Data.Repositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<T?> GetByIdWithIncludeAsync(Guid id, CancellationToken ct = default, params Expression<Func<T, object>>[] includes);
        Task<PagedResult<T>> GetPagedResultAsync(PagedRequest request, CancellationToken ct);
        Task AddAsync(T entity, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<T> objModel, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(T entity, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct);
        IQueryable<T> GetAll();
    }
}

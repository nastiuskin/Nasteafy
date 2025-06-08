using Nasteafy.Domain.Base;
using Nasteafy.Domain.Contracts;

namespace Nasteafy.Application.Abstractions
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        IQueryable<T> GetAll();
        Task AddAsync(T entity, CancellationToken ct);
        Task AddRange(IEnumerable<T> objModel, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(T entity, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
        Task<bool> ExistsAsync(Guid id,CancellationToken ct);
    }   
}

using Nasteafy.Domain.Base;
using Nasteafy.Domain.Contracts;

namespace Nasteafy.Application.Abstractions
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        IQueryable<T> GetAll(CancellationToken ct);
        Task AddAsync(T entity, CancellationToken ct);
        Task AddRange(IEnumerable<T> objModel);
        Task DeleteAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(T entity, CancellationToken ct);
        Task<int> CountAsync();
    }   
}

using Nasteafy.Domain.Contracts;

namespace Nasteafy.Application.Abstractions
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        IQueryable<T> GetAll(CancellationToken ct);
        Task AddAsync(T entity, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(T entity, CancellationToken ct);
    }
}

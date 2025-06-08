using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Base;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task AddAsync(T entity, CancellationToken ct)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public async Task AddRange(IEnumerable<T> objModel, CancellationToken ct)
        {
            await _dbSet.AddRangeAsync(objModel, ct);
        }

        public async Task<int> CountAsync(CancellationToken ct)
        {
            return await _dbSet.CountAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var entity = await GetByIdAsync(id, ct);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Guid id,CancellationToken ct)
        {
            var entity = await GetByIdAsync(id, ct);
            return entity is not null;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task UpdateAsync(T entity, CancellationToken ct)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}

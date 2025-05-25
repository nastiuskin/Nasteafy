using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions;
using Nasteafy.Domain.Contracts;

namespace Nasteafy.Infrastructure.Database.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly DatabaseContext _context;

        protected BaseRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task AddAsync(T entity, CancellationToken ct)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public async Task AddRange(IEnumerable<T> objModel)
        {
            await _dbSet.AddRangeAsync(objModel);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var entity = await GetByIdAsync(id, ct);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public IQueryable<T> GetAll(CancellationToken ct)
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task UpdateAsync(T entity, CancellationToken ct)
        {
             _dbSet.Update(entity);
             return Task.CompletedTask;
        }
    }
}

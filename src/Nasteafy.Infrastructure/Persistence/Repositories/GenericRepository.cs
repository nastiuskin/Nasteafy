using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Abstractions.Data.Repositories;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Base;
using Nasteafy.Infrastructure.Persistence.Contexts;
using Nasteafy.Infrastructure.Persistence.Extensions;
using System.Linq.Expressions;

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

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct)
        {
            await _dbSet.AddRangeAsync(entities, ct);
        }

        public async Task<int> CountAsync(CancellationToken ct)
        {
            return await _dbSet.CountAsync(ct);
        }

        public async Task DeleteAsync(T entity, CancellationToken ct)
        {
            if (entity != null)
                 _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }   

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        private IQueryable IncludeProperties(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> entities = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }
            return entities;
        }

        public Task UpdateAsync(T entity, CancellationToken ct)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<Application.Common.Models.PagedResult<T>> GetPagedResultAsync(PagedRequest request, CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking();

            return await query.ToPagedResultAsync(request, ct);
        }

        public async Task<T?> GetByIdWithIncludeAsync(Guid id, CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }
    }
}

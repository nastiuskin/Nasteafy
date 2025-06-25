using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Base;
using System.Linq.Dynamic.Core;

namespace Nasteafy.Infrastructure.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int pageSize) =>
            query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        public static async Task<Application.Common.Models.PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            PagedRequest request,
            CancellationToken ct = default) where T : IEntity
        {
            query = query.OrderBy(x => x.Id);

            var totalItems = await query.CountAsync(ct);

            var items = await query
                .ApplyPaging(request.PageNumber, request.PageSize)
                .ToListAsync(ct);

            return new Application.Common.Models.PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}

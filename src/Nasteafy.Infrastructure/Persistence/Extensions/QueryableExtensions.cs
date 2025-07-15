using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain.Base;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Nasteafy.Infrastructure.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        private static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int pageSize) =>
            query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        private static IQueryable<T> Sort<T>(this IQueryable<T> query, PagedRequest pagedRequest) where T : IEntity
        {
            if (!string.IsNullOrWhiteSpace(pagedRequest.SortBy))
            {
                query = query.OrderBy(pagedRequest.SortBy + " " + pagedRequest.SortDirection);
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }
            return query;
        }

        private static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, PagedRequest pagedRequest) where T : IEntity
        {
            var predicate = new StringBuilder();
            var requestFilters = pagedRequest.RequestFilters;

            if(requestFilters is null || !requestFilters.Filters.Any())
            {
                return query;
            }

            for (int i = 0; i < requestFilters.Filters.Count; i++)
            {
                if (i > 0)
                {
                    predicate.Append($" {requestFilters.LogicalOperator} ");
                }
                predicate.Append(requestFilters.Filters[i].Path + $".{nameof(string.Contains)}(@{i})");
            }

            if (requestFilters.Filters.Any())
            {
                var propertyValues = requestFilters.Filters.Select(filter => filter.Value).ToArray();

                query = query.Where(predicate.ToString(), propertyValues);
            }

            return query;
        }

        public static async Task<Application.Common.Models.PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            PagedRequest request,
            CancellationToken ct = default) where T : IEntity
        {
            query = query.ApplyFilters(request).Sort(request);

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

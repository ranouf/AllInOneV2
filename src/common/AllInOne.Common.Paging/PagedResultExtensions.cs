using AllInOne.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AllInOne.Common.Paging
{
    public static class PagedResultExtensions
    {
        public async static Task<PagedResult<TEntity>> ToPagedResultAsync<TEntity>(
            this IQueryable<TEntity> source,
            int? maxResultCount = null,
            int? skipCount = null
        ) where TEntity : IEntity
        {
            var totalCount = await source.CountAsync();

            bool hasNext = false;
            if (!maxResultCount.HasValue && skipCount.HasValue)
            {
                hasNext = totalCount > skipCount;
            }
            else if (maxResultCount.HasValue && skipCount.HasValue)
            {
                hasNext = totalCount > (skipCount + maxResultCount);
            }

            var takeCount = (!maxResultCount.HasValue || maxResultCount.Value < 0)
                ? totalCount
                : maxResultCount.Value;
            takeCount = (takeCount < 1)
                ? 1
                : takeCount;

            var result = await source
                .Skip(skipCount ?? 0)
                .Take(takeCount)
                .ToListAsync();

            return new PagedResult<TEntity>(totalCount, result, hasNext);
        }

        public async static Task<PagedResult<TEntity, TPrimaryKey>> ToPagedResultAsync<TEntity, TPrimaryKey>(
            this IQueryable<TEntity> source,
            int? maxResultCount = null,
            int? skipCount = null
        ) where TEntity : IEntity<TPrimaryKey>
        {
            var totalCount = await source.CountAsync();

            bool hasNext = false;
            if (!maxResultCount.HasValue && skipCount.HasValue)
            {
                hasNext = totalCount > skipCount;
            }
            else if (maxResultCount.HasValue && skipCount.HasValue)
            {
                hasNext = totalCount > (skipCount + maxResultCount);
            }

            var takeCount = (!maxResultCount.HasValue || maxResultCount.Value < 0)
                ? totalCount
                : maxResultCount.Value;
            takeCount = (takeCount < 1)
                ? 1
                : takeCount;

            var result = await source
                .Skip(skipCount ?? 0)
                .Take(takeCount)
                .ToListAsync();

            return new PagedResult<TEntity, TPrimaryKey>(totalCount, result, hasNext);
        }
    }
}

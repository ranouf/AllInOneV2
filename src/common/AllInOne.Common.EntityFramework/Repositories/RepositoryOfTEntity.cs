using AllInOne.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace AllInOne.Common.EntityFramework.Repositories
{
    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
      where TEntity : class, IEntity<Guid>
    {
        public Repository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}

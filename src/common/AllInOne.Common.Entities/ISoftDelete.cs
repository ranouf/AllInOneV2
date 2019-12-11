using System;

namespace AllInOne.Common.Entities
{
    public interface ISoftDelete<TUser> : ISoftDelete<Guid?, TUser>, IDeleteAudited<TUser>
    {
    }
    public interface ISoftDelete<TPrimaryKey, TUser> : IDeleteAudited<TPrimaryKey, TUser>
    {
        bool IsDeleted { get; set; }
    }
}

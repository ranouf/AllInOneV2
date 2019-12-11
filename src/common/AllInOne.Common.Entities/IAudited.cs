using System;

namespace AllInOne.Common.Entities
{
    public interface IAudited<TUser> : IAudited<Guid?, TUser>, ICreationAudited<TUser>, IUpdateAudited<TUser>
    {
    }

    public interface IAudited<TPrimaryKey, TUser> : ICreationAudited<TPrimaryKey, TUser>, IUpdateAudited<TPrimaryKey, TUser>
    {
    }
}

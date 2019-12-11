using System;

namespace AllInOne.Common.Entities
{
    public interface ICreationAudited<TUser> : ICreationAudited<Guid?, TUser>
    {
    }

    public interface ICreationAudited<TPrimaryKey, TUser>
    {
        DateTimeOffset? CreatedAt { get; set; }
        TPrimaryKey CreatedByUserId { get; set; }
        TUser CreatedByUser { get; set; }
    }
}

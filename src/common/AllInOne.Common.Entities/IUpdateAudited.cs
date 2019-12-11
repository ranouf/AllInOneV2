using System;

namespace AllInOne.Common.Entities
{
    public interface IUpdateAudited<TUser> : IUpdateAudited<Guid?, TUser>
    {
    }

    public interface IUpdateAudited<TPrimaryKey, TUser>
    {
        DateTimeOffset? UpdatedAt { get; set; }
        TPrimaryKey UpdatedByUserId { get; set; }
        TUser UpdatedByUser { get; set; }
    }
}

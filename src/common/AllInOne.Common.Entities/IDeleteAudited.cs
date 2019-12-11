using System;

namespace AllInOne.Common.Entities
{
    public interface IDeleteAudited<TUser> : IDeleteAudited<Guid?, TUser>
    {
    }
    public interface IDeleteAudited<TPrimaryKey, TUser>
    {
        DateTimeOffset? DeletedAt { get; set; }
        TPrimaryKey DeletedByUserId { get; set; }
        TUser DeletedByUser { get; set; }
    }
}

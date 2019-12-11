using System;

namespace AllInOne.Common.Session
{
    public interface IUserSession
    {
        Guid? UserId { get; }
        string BaseUrl { get; }
    }
}

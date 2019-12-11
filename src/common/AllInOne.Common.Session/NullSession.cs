using System;

namespace AllInOne.Common.Session
{
    public class NullSession : IUserSession
    {
        public Guid? UserId => null;

        public string BaseUrl => null;
    }
}

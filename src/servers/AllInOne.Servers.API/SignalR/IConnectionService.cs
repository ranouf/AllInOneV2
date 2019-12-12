using System.Collections.Generic;

namespace AllInOne.Api.SignalR
{
    public interface IConnectionService
    {
        void Add(string userId, string connectionId);
        void Remove(string userId);
        IReadOnlyList<string> GetAllExcept(string userId);
    }
}
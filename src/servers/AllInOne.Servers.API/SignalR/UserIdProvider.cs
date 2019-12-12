using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;

namespace AllInOne.Api.SignalR
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}

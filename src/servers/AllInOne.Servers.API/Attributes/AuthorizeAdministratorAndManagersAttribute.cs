using AllInOne.Domains.Core;
using Microsoft.AspNetCore.Authorization;

namespace AllInOne.Servers.API.Attributes
{
    public class AuthorizeAdministratorAndManagersAttribute : AuthorizeAttribute
    {
        public AuthorizeAdministratorAndManagersAttribute()
        {
            Roles = $"{Constants.Roles.Administrator},{Constants.Roles.Manager}";
        }
    }
}

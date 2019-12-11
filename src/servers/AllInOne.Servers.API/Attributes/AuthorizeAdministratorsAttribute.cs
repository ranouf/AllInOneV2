using AllInOne.Domains.Core;
using Microsoft.AspNetCore.Authorization;

namespace AllInOne.Servers.API.Attributes
{
    public class AuthorizeAdministratorsAttribute : AuthorizeAttribute
    {
        public AuthorizeAdministratorsAttribute()
        {
            Roles = Constants.Roles.Administrator;
        }
    }
}

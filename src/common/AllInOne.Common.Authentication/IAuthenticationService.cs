using AllInOne.Domains.Core.Identity.Entities;

namespace AllInOne.Common.Authentication
{
    public interface IAuthenticationService
    {
        string GenerateToken(User user);
    }
}

using AllInOne.Domains.Core.Identity.Entities;
using System.Threading.Tasks;

namespace AllInOne.Domains.Core.Emails
{
    public interface IEmailManager
    {
        Task SendPasswordForgottenEmailAsync(User user, string token);
        Task SendInviteUserEmailAsync(User user, string token);
        Task SendConfirmEmailAsync(User user, string token);
    }
}
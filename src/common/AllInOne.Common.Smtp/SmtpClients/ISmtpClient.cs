using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AllInOne.Common.Smtp.SmtpClients
{
    public interface ISmtpClient
    {
        bool UseDefaultCredentials { get; set; }
        bool EnableSsl { get; set; }
        ICredentialsByHost Credentials { get; set; }

        public Task SendMailAsync(MailMessage message);
    }
}

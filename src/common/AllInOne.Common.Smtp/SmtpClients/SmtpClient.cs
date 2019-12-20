using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AllInOne.Common.Smtp.SmtpClients
{
    internal class SmtpClient : ISmtpClient
    {
        private readonly System.Net.Mail.SmtpClient _smtpClient;

        public bool UseDefaultCredentials
        {
            get => _smtpClient.UseDefaultCredentials;
            set => _smtpClient.UseDefaultCredentials = value;
        }
        public bool EnableSsl
        {
            get => _smtpClient.EnableSsl;
            set => _smtpClient.EnableSsl = value;
        }
        public ICredentialsByHost Credentials
        {
            get => _smtpClient.Credentials;
            set => _smtpClient.Credentials = value;
        }


        public SmtpClient(string host, int port)
        {
            _smtpClient = new System.Net.Mail.SmtpClient(host, port);
        }

        public async Task SendMailAsync(MailMessage message)
        {
            await _smtpClient.SendMailAsync(message);
        }
    }
}

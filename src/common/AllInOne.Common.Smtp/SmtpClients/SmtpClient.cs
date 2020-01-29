using MimeKit;
using System.Threading.Tasks;

namespace AllInOne.Common.Smtp.SmtpClients
{
    internal class SmtpClient : ISmtpClient
    {
        private readonly MailKit.Net.Smtp.SmtpClient _smtpClient;

        public bool IsConnected
        {
            get
            {
                return _smtpClient.IsConnected;
            }
        }

        public SmtpClient()
        {
            _smtpClient = new MailKit.Net.Smtp.SmtpClient();
        }

        public async Task ConnectAsync(string host, int port, bool useSsl)
        {
            await _smtpClient.ConnectAsync(host, port, useSsl);
        }

        public async Task AuthenticateAsync(string username, string password)
        {
            await _smtpClient.AuthenticateAsync(username, password);
        }

        public async Task SendAsync(MimeMessage message)
        {
            await _smtpClient.SendAsync(message);
        }

        public async ValueTask DisposeAsync()
        {
            await _smtpClient.DisconnectAsync(true);
        }
    }
}

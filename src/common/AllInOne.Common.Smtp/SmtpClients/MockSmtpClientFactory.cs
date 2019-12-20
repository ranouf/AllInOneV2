using Moq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AllInOne.Common.Smtp.SmtpClients
{
    public class MockSmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient CreateSmtpClient(string host, int port)
        {
            return new Mock<ISmtpClient>().Object;
        }
    }
}

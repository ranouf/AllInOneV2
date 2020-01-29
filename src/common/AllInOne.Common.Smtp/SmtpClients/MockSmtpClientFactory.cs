using Moq;

namespace AllInOne.Common.Smtp.SmtpClients
{
    public class MockSmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient CreateSmtpClient()
        {
            return new Mock<ISmtpClient>().Object;
        }
    }
}

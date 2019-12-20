namespace AllInOne.Common.Smtp.SmtpClients
{
    public class SmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient CreateSmtpClient(string host, int port)
        {
            return new SmtpClient(host, port);
        }
    }
}

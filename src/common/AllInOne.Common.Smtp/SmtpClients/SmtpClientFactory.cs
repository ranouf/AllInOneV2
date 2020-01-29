namespace AllInOne.Common.Smtp.SmtpClients
{
    public class SmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient CreateSmtpClient()
        {
            return new SmtpClient();
        }
    }
}

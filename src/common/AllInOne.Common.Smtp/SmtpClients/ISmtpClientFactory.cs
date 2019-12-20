namespace AllInOne.Common.Smtp.SmtpClients
{
    public interface ISmtpClientFactory
    {
        ISmtpClient CreateSmtpClient(string host, int port);
    }
}
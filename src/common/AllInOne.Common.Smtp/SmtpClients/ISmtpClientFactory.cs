namespace AllInOne.Common.Smtp.SmtpClients
{
    public interface ISmtpClientFactory
    {
        ISmtpClient CreateSmtpClient();
    }
}
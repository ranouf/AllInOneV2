using AllInOne.Common.Extensions;
using AllInOne.Common.Logging;
using AllInOne.Common.Smtp.Configuration;
using AllInOne.Common.Smtp.SmtpClients;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AllInOne.Common.Smtp.Extensions;

namespace AllInOne.Common.Smtp
{
    public class SmtpService : ISmtpService
    {
        private readonly ISmtpClientFactory _smtpClientFactory;
        private readonly SmtpSettings _smtpSettings;
        private readonly ILoggerService<SmtpService> _logger;

        public SmtpService(
            [NotNull]ISmtpClientFactory smtpClientFactory,
            [NotNull]IOptions<SmtpSettings> smtpSettings,
            ILoggerService<SmtpService> logger
        )
        {
            _smtpClientFactory = smtpClientFactory;
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = GenerateEmail(to, subject, body);

            try
            {
                await using (var smtpClient = _smtpClientFactory.CreateSmtpClient())
                {
                    await smtpClient.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, _smtpSettings.EnableSsl);

                    await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

                    await smtpClient.SendAsync(email);
                }

                _logger.LogInformation($"[{nameof(SmtpService)}] Email has been sent. email: {email.ToJson()}");
            }
            catch (Exception e)
            {
                _logger.LogError($"[{nameof(SmtpService)}] Email sending failed. email: {email.ToJson()}", e);
                throw;
            }
        }

        private MimeMessage GenerateEmail(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_smtpSettings.DefaultFrom));
            email.To.Add(new MailboxAddress(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            return email;
        }
    }
}

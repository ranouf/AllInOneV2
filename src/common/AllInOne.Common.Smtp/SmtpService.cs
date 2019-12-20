using AllInOne.Common.Extensions;
using AllInOne.Common.Logging;
using AllInOne.Common.Smtp.Configuration;
using AllInOne.Common.Smtp.SmtpClients;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

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
            var email = new MailMessage
            {
                From = new MailAddress(_smtpSettings.DefaultFrom),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            email.To.Add(to);
            var client = _smtpClientFactory.CreateSmtpClient(_smtpSettings.Server, _smtpSettings.Port);
            client.UseDefaultCredentials = false;
            client.EnableSsl = _smtpSettings.EnableSsl;
            client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

            try
            {
                _logger.LogInformation($"[{nameof(SmtpService)}] An email will be send. email: {email.ToJson()}");
                await client.SendMailAsync(email);
                _logger.LogInformation($"[{nameof(SmtpService)}] Email has been sent. email: {email.ToJson()}");
            }
            catch (Exception e)
            {
                _logger.LogError($"[{nameof(SmtpService)}] Email sending failed. email: {email.ToJson()}", e);
                throw;
            }
        }
    }
}

using AllInOne.Common.Settings.HealthChecks;
using AllInOne.Common.Smtp.Configuration;
using AllInOne.Common.Smtp.SmtpClients;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace AllInOne.Common.Smtp.HealthChecks
{
    public class SmtpHealthCheck: SettingsHealthCheck<SmtpSettings>
    {
        private readonly ISmtpClientFactory _smtpClientFactory;

        public SmtpHealthCheck(
            [NotNull]ISmtpClientFactory smtpClientFactory,
            [NotNull]IOptions<SmtpSettings> options
        ): base(options)
        {
            _smtpClientFactory = smtpClientFactory;
        }

        public async override Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // Settings Validation
            var result = await base.CheckHealthAsync(context, cancellationToken);
            if (result.Status == HealthStatus.Unhealthy)
            {
                return result;
            }

            // SMTP Connection Validation
            await using (var smtpClient = _smtpClientFactory.CreateSmtpClient())
            {
                try
                {

                    await smtpClient.ConnectAsync(Settings.Server, Settings.Port, Settings.EnableSsl);
                }
                catch (Exception e)
                {
                    return await Task.FromResult(HealthCheckResult.Unhealthy(
                        description: $"Smtp Connection failed to Server:'{Settings.Server}' with Error: '{e.Message}'",
                        exception: e
                    ));
                }

                try
                {
                    await smtpClient.AuthenticateAsync(Settings.Username, Settings.Password);
                }
                catch (Exception e)
                {
                    return await Task.FromResult(HealthCheckResult.Unhealthy(
                        description: $"Smtp Connection failed to Server:'{Settings.Server}' with Error: '{e.Message}'",
                        exception: e
                    ));
                }

                if (!smtpClient.IsConnected)
                {
                    return await Task.FromResult(HealthCheckResult.Unhealthy(
                        description: $"Smtp Connection failed to Server:'{Settings.Server}'"
                    ));
                }
            }
            return await Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}

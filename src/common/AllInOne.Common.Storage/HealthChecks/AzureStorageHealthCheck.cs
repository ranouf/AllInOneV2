using AllInOne.Common.Storage.Configuration;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace AllInOne.Common.Storage.HealthChecks
{
    public class AzureStorageHealthCheck : IHealthCheck
    {
        private readonly AzureStorageSettings _azureSettings;

        public AzureStorageHealthCheck(
            [NotNull]IOptions<AzureStorageSettings> azureSettings
        )
        {
            _azureSettings = azureSettings.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var client = new BlobContainerClient(_azureSettings.ConnectionString, _azureSettings.Container);

                var properties = await client.GetPropertiesAsync(
                   cancellationToken: cancellationToken
                );

                if (!await client.ExistsAsync())
                {
                    return new HealthCheckResult(context.Registration.FailureStatus, description: $"Container '{_azureSettings.Container}' does not exist.");
                }

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}

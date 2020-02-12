using AllInOne.Common.Storage.BlobContainerClients;
using AllInOne.Common.Storage.Configuration;
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
        private readonly IBlobContainerClientFactory _blobContainerClientFactory;
        private readonly AzureStorageSettings _azureSettings;

        public AzureStorageHealthCheck(
            [NotNull]IBlobContainerClientFactory blobContainerClientFactory,
            [NotNull]IOptions<AzureStorageSettings> azureSettings
        )
        {
            _blobContainerClientFactory = blobContainerClientFactory;
            _azureSettings = azureSettings.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {

                var client = _blobContainerClientFactory.CreateBlobContainerClient(
                    _azureSettings.ConnectionString,
                    _azureSettings.Container
                );

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

﻿using AllInOne.Common.Logging;
using AllInOne.Common.Storage.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AllInOne.Common.Storage
{
    public class AzureStorageService : IStorageService
    {
        private readonly AzureStorageSettings _azureSettings;
        private readonly ILoggerService<AzureStorageService> _logger;

        private BlobContainerClient _blobContainerClient;
        private BlobContainerClient Client
        {
            get
            {
                if (_blobContainerClient == null)
                {
                    _blobContainerClient = new BlobContainerClient(_azureSettings.ConnectionString, _azureSettings.Container);
                }
                return _blobContainerClient;
            }
        }

        public AzureStorageService(
            [NotNull]IOptions<AzureStorageSettings> azureSettings,
            ILoggerService<AzureStorageService> logger
        )
        {
            _azureSettings = azureSettings.Value;
            _logger = logger;
        }

        public async Task CreateIfNotExistsAsync()
        {
            if (!await Client.ExistsAsync())
            {
                await Client.CreateAsync(PublicAccessType.Blob);
                _logger.LogInformation($"BlobContainer '{_azureSettings.Container}' has been created.");
            }
        }

        public async Task DeleteAsync()
        {
            await Client.DeleteAsync();
            _logger.LogInformation($"BlobContainer '{_azureSettings.Container}' has been deleted.");
        }

        public async Task<Uri> SaveFileAsync(Stream stream, string fileName)
        {
            var path = fileName.ToLower();
            var blob = Client.GetBlobClient(path);
            await blob.UploadAsync(stream);
            _logger.LogInformation($"'{fileName}' has been saved in '{blob.Uri.AbsoluteUri}'.");
            return blob.Uri;
        }

        public async Task RemoveFileAsync(string fileName)
        {
            var path = fileName.ToLower();
            var blob = Client.GetBlobClient(path);
            if (!await blob.ExistsAsync())
            {
                _logger.LogWarning($"Delete file '{fileName}' failed because the file does not exist.");
                throw new FileNotFoundException();
            }

            var response = await blob.DeleteAsync();
            if (response.Status != (int)HttpStatusCode.Accepted)
            {
                var errorMessage = $"Delete file '{fileName}' failed with ReasonPhrase: '{response.ReasonPhrase}'.";
                _logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            _logger.LogInformation($"'{fileName}' has been deleted.");
        }
    }
}

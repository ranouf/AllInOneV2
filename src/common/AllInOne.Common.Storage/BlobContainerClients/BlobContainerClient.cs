using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AllInOne.Common.Storage.BlobContainerClients
{
    internal class BlobContainerClient : IBlobContainerClient
    {

        private readonly Azure.Storage.Blobs.BlobContainerClient _blobContainerClient;

        public BlobContainerClient(string connectionString, string blobContainerName)
        {
            _blobContainerClient = new Azure.Storage.Blobs.BlobContainerClient(connectionString, blobContainerName);
        }

        public Task<Response<bool>> ExistsAsync()
        {
            return _blobContainerClient.ExistsAsync();
        } 

        public Task<Response<BlobContainerInfo>> CreateAsync(PublicAccessType publicAccessType = PublicAccessType.None)
        {
            return _blobContainerClient.CreateAsync(publicAccessType);
        }

        public BlobClient GetBlobClient(string blobName)
        {
            return _blobContainerClient.GetBlobClient(blobName);
        }

        public Task<Response<BlobContainerProperties>> GetPropertiesAsync(BlobRequestConditions conditions = null, CancellationToken cancellationToken = default)
        {
            return _blobContainerClient.GetPropertiesAsync(conditions, cancellationToken);
        }
    }
}

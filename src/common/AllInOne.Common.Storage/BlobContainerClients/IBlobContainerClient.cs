using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AllInOne.Common.Storage.BlobContainerClients
{
    public interface IBlobContainerClient
    {
        Task<Response<BlobContainerInfo>> CreateAsync(PublicAccessType publicAccessType = PublicAccessType.None);
        Task<Response<bool>> ExistsAsync();
        BlobClient GetBlobClient(string blobName);
        Task<Response<BlobContainerProperties>> GetPropertiesAsync(BlobRequestConditions conditions = null, CancellationToken cancellationToken = default);
    }
}

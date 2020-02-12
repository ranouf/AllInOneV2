namespace AllInOne.Common.Storage.BlobContainerClients
{
    public interface IBlobContainerClientFactory
    {
        IBlobContainerClient CreateBlobContainerClient(string connectionString, string blobContainerName);
    }
}

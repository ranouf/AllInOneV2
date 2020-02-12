namespace AllInOne.Common.Storage.BlobContainerClients
{
    public class BlobContainerClientFactory : IBlobContainerClientFactory
    {
        public IBlobContainerClient CreateBlobContainerClient(string connectionString, string blobContainerName)
        {
            return new BlobContainerClient(
                connectionString,
                blobContainerName
            );
        }
    }
}

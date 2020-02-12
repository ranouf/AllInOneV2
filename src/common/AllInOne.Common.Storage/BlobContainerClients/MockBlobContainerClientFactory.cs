using Azure.Storage.Blobs;
using Moq;
using System;

namespace AllInOne.Common.Storage.BlobContainerClients
{
    public class MockBlobContainerClientFactory : IBlobContainerClientFactory
    {
        public IBlobContainerClient CreateBlobContainerClient(string connectionString, string blobContainerName)
        {
            var mockBlockClient = new Mock<BlobClient>(MockBehavior.Loose);
            mockBlockClient
                .SetupGet(client => client.Uri)
                .Returns(new Uri("http://allinone.png"));

            var result = new Mock<IBlobContainerClient>(MockBehavior.Loose);
            result
                .Setup(client => client.GetBlobClient(It.IsAny<string>()))
                .Returns(mockBlockClient.Object);

            return result.Object;
        }
    }
}

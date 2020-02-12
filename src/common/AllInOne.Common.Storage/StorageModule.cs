using AllInOne.Common.Storage;
using AllInOne.Common.Storage.BlobContainerClients;
using Autofac;

namespace AllInOne.Common.Storage
{
    public class StorageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AzureStorageService>().As<IStorageService>();
            builder.RegisterType<BlobContainerClientFactory>().As<IBlobContainerClientFactory>();
        }
    }
}

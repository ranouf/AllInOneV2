using Autofac;

namespace AllInOne.Common.Storage
{
    public class StorageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AzureStorageService>().As<IStorageService>();
        }
    }
}

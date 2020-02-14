using AllInOne.Common.Events;
using AllInOne.Common.Smtp;
using AllInOne.Common.Storage;
using Autofac;
using System.Reflection;

namespace AllInOne.Domain.Core
{
    public class CoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var core = typeof(CoreModule).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(core)
                   .Where(t => t.Name.EndsWith("Manager"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
            builder.RegisterModule<EventsModule>();
            builder.RegisterModule<SmtpModule>();
            builder.RegisterModule<StorageModule>();
        }
    }
}

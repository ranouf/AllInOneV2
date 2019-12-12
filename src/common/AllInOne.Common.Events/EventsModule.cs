using Autofac;

namespace AllInOne.Common.Events
{
    public class EventsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEvents>().As<IDomainEvents>();
        }
    }
}

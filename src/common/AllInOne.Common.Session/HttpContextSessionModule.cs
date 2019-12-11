using Autofac;

namespace AllInOne.Common.Session
{
    public class HttpContextSessionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextSession>().As<IUserSession>();
        }
    }
}

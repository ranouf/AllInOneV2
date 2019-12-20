using AllInOne.Common.Smtp.SmtpClients;
using Autofac;

namespace AllInOne.Common.Smtp
{
    public class SmtpModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmtpService>().As<ISmtpService>();
            builder.RegisterType<SmtpClientFactory>().As<ISmtpClientFactory>();
        }
    }
}

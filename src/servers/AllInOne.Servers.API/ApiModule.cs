using AllInOne.Common.Authentication;
using AllInOne.Common.Logging;
using AllInOne.Common.Session;
using AllInOne.Common.Settings;
using AllInOne.Domain.Core;
using AllInOne.Domain.Infrastructure;
using AllInOne.Servers.API.Filters;
using Autofac;
using Microsoft.AspNetCore.Http;

namespace AllInOne.Servers.API
{
    public class ApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            builder.RegisterType<ApiExceptionFilter>();
            builder.RegisterType<SettingsValidator>();

            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<AuthenticationModule>(); 
            builder.RegisterModule<LoggingModule>();
            builder.RegisterModule<HttpContextSessionModule>();
        }
    }
}

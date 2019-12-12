using AllInOne.Api.SignalR;
using AllInOne.Common.Authentication;
using AllInOne.Common.Events;
using AllInOne.Common.Logging;
using AllInOne.Common.Session;
using AllInOne.Common.Settings;
using AllInOne.Domain.Core;
using AllInOne.Domain.Infrastructure;
using AllInOne.Servers.API.Filters;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            // SignalR
            builder.RegisterType<ConnectionService>().As<IConnectionService>().SingleInstance();
            IEnumerable<Assembly> assemblies = GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();
        }

        private static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var libraries = DependencyContext.Default.RuntimeLibraries.Where(rl => rl.Name.StartsWith("AllInOne"));
            foreach (var library in libraries)
            {
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                assemblies.Add(assembly);
            }
            return assemblies.ToArray();
        }
    }
}

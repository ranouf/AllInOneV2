using AllInOne.Common.Smtp.SmtpClients;
using AllInOne.Domains.Infrastructure.SqlServer;
using AllInOne.Servers.API;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AllInOne.Integration.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
            // nothing
        }

        public override void SetUpDataBase(IServiceCollection services)
        {
            services.AddDbContext<AllInOneDbContext>(options => options
                .UseInMemoryDatabase("AllInOne")
                .EnableSensitiveDataLogging()
            );
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterType<MockSmtpClientFactory>().As<ISmtpClientFactory>();
        }
    }
}

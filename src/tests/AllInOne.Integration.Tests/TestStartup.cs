using AllInOne.Domains.Infrastructure.SqlServer;
using AllInOne.Servers.API;
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
    }
}

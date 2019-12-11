using AllInOne.Domains.Core.Identity;
using AllInOne.Domains.Infrastructure.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests.Data
{
    public static class TestDbInitializer
    {
        public static void Seed(IServiceProvider services, ITestOutputHelper output)
        {
            try
            {
                var context = services.GetRequiredService<AllInOneDbContext>();
                if (context.Database.EnsureCreated())
                {

                    var userManager = services.GetRequiredService<IUserManager>();
                    var roleManager = services.GetRequiredService<IRoleManager>();

                    new TestUserDataBuilder(context, userManager, roleManager, output).Seed();

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                output.WriteLine(ex.Message);
                throw;
            }
        }
    }
}

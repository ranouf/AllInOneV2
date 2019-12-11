using AllInOne.Common.Testing.Xunit;
using AllInOne.Integration.Tests.Data;
using AllInOne.Integration.Tests.Extensions;
using AllInOne.Servers.API;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests
{
    public class TestServerFixture : IDisposable
    {
        public IHost Host { get; private set; }
        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }
        public ITestOutputHelper Output { get; private set; }

        public TestServerFixture()
        {

        }

        public TestServerFixture(ITestOutputHelper output)
        {
            Output = output ?? throw new ArgumentNullException(nameof(output));

            var hostBuilder = new HostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddXunit(Output);
                })
                .ConfigureWebHost(webHost =>
                {
                    webHost
                        .UseStartup<TestStartup>()
                        .BasedOn<Startup>() //Internal extension to re set the correct ApplicationKey
                        .UseTestServer();
                });

            Host = hostBuilder.Start();
            Server = Host.GetTestServer();
            Client = Host.GetTestClient();

            using (var scope = Host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                TestDbInitializer.Seed(services, Output);
            }
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
            Host.Dispose();
        }
    }
}

using AllInOne.Integration.Tests.Data;
using AllInOne.Integration.Tests.Extensions;
using AllInOne.Integration.Tests.InMemory;
using AllInOne.Integration.Tests.ObservableConfiguration;
using AllInOne.Integration.Tests.Xunit;
using AllInOne.Servers.API;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Subjects;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests
{
    public class TestServerFixture : IDisposable
    {
        public IHost Host { get; private set; }
        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }
        public ITestOutputHelper Output { get; private set; }
        public List<string> Logs { get; private set; } = new List<string>();

        private List<KeyValuePair<string, string>> _initialData;
        private BehaviorSubject<IEnumerable<KeyValuePair<string, string>>> _configuration;

        public TestServerFixture()
        {
        }

        public TestServerFixture([NotNull]ITestOutputHelper output)
        {
            Output = output ?? throw new ArgumentNullException(nameof(output));

            _initialData = new List<KeyValuePair<string, string>>();
            _configuration = new BehaviorSubject<IEnumerable<KeyValuePair<string, string>>>(_initialData);

            var hostBuilder = new HostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, builder) =>
                {
                    builder
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .AddObservableConfiguration(_configuration);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddXunitLogger(Output);
                    builder.AddInMemoryLogger(Logs);
                })
                .ConfigureWebHost(builder =>
                {
                    builder
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

        public void AddToConfiguration(string key, string value)
        {
            _initialData = _initialData
                .Where(d => d.Key != key)
                .ToList();
            _initialData.Add(new KeyValuePair<string, string>(key, value));
            _configuration.OnNext(_initialData);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client.Dispose();
                Server.Dispose();
                Host.Dispose();
            }
        }
    }
}

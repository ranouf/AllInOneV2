using AllInOne.Common.Logging;
using AllInOne.Common.Settings;
using AllInOne.Common.Storage;
using AllInOne.Domains.Infrastructure.SqlServer;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AllInOne.Servers.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILoggerService<Program>>();

                ValidateSettings(services, logger);
                await InitializeDataBaseAsync(services, logger);
                await InitializeStorage(services, logger);

                try
                {
                    logger.LogInformation("Api starting.");
                    await webHost.RunAsync();
                }
                catch (Exception e)
                {
                    logger.LogError("Api failed to start.", e);
                    throw;
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    if (env.IsDevelopment())
                    {
                        config.SetBasePath(env.ContentRootPath)
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    }
                    config.AddCommandLine(args);
                    config.AddEnvironmentVariables();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddEventSourceLogger();

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddDebug();
                        logging.AddConsole();
                    }
                    else
                    {
                        logging.AddAzureWebAppDiagnostics();
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        #region Private
        private static void ValidateSettings(IServiceProvider services, ILoggerService<Program> logger)
        {
            try
            {
                logger.LogInformation("Starting settings validation.");
                services.GetRequiredService<SettingsValidator>();
                logger.LogInformation("The settings has been validated.");
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while validating the settings.", ex);
                throw;
            }
        }

        private static async Task InitializeDataBaseAsync(IServiceProvider services, ILoggerService<Program> logger)
        {
            try
            {
                logger.LogInformation("Starting the database initialization.");
                await DbInitializer.InitializeAsync(services, logger);
                logger.LogInformation("The database initialization has been done.");
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while initializing the database.", ex);
                throw;
            }
        }

        private static async Task InitializeStorage(IServiceProvider services, ILoggerService<Program> logger)
        {
            try
            {
                logger.LogInformation("Starting the storage initialization.");
                var storageService = services.GetRequiredService<IStorageService>();
                await storageService.CreateIfNotExistsAsync();
                logger.LogInformation("The storage initialization has been done.");
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while initializing the storage.", ex);
                throw;
            }
        }
        #endregion
    }
}

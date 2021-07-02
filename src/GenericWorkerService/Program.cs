using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GenericWorkerService
{
    public class Program
    {
        private const string AppName = "Generic Worker Service";

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

            try
            {
                IHost host = CreateHostBuilder(args).Build();

                Log.Information("{AppName} :: Start", AppName);

                await host.RunAsync();
            }
            catch (Exception exception)
            {
                Log.Error(exception, "{AppName} :: Stopped on Main", AppName);
                throw;
            }
            finally
            {
                Log.Information("{AppName} :: End", AppName);
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile("appsettings.local.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    Startup startup = new(hostContext.Configuration);
                    startup.ConfigureServices(services);
                })
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                });
        }
    }
}

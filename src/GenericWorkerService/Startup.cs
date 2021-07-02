using GenericWorkerService.BusinessLayer.Extentions;
using GenericWorkerService.InfrastructureLayer.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GenericWorkerService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingSection = Configure<AppSettings>(nameof(AppSettings));

            services.AddGenericWorkerServices(
                options =>
                {
                    options.IsEnable = appSettingSection.GenericWorkerIsEnable;
                    options.PollingFrequency = appSettingSection.GenericWorkerPollingFrequency;
                    options.WaitBeforeStart = appSettingSection.GenericWorkerWaitBeforeStart;
                });

            T Configure<T>(string sectionName) where T : class
            {
                var section = Configuration.GetSection(sectionName);
                var settings = section.Get<T>();
                services.Configure<T>(section);

                return settings;
            }
        }
    }
}

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
            AppSettings appSettingSection = Configure<AppSettings>(nameof(AppSettings));

            services.AddGenericWorkerServices(
                options =>
                {
                    options.IsEnable = appSettingSection.GenericWorkerIsEnable;
                    options.PollingFrequency = appSettingSection.GenericWorkerPollingFrequency;
                    options.WaitBeforeStart = appSettingSection.GenericWorkerWaitBeforeStart;
                });

            services.AddHostedService<BusinessLayer.Services.GenericWorkerService>();

            T Configure<T>(string sectionName) where T : class
            {
                IConfigurationSection section = Configuration.GetSection(sectionName);
                T settings = section.Get<T>();
                services.Configure<T>(section);

                return settings;
            }
        }
    }

}

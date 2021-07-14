using GenericWorkerService.BusinessLayer.Extentions;
using GenericWorkerService.BusinessLayer.Settings;
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
            var appSettings = Configure<AppSettings>(nameof(AppSettings));

            services.AddGenericService(
                (ref MainServiceSettings options) =>
                {
                    options = appSettings.MainService.Clone() as MainServiceSettings;
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

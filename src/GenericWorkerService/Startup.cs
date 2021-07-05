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
            var settings = Configure<AppSettings>(nameof(AppSettings));

            services.AddGenericWorkerServices(
                (ref GenericWorkerSettings options) =>
                {
                    options = settings.GenericWorker.Clone() as GenericWorkerSettings;
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

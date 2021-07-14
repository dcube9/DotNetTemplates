using GenericWorkerService.BusinessLayer.Services;
using GenericWorkerService.BusinessLayer.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace GenericWorkerService.BusinessLayer.Extentions
{
    public delegate void ActionRef<T>(ref T item);
    public delegate void ActionOut<T>(out T item);

    public static class MainServiceExtensions
    {
        public static IServiceCollection AddGenericService(this IServiceCollection services, ActionRef<MainServiceSettings> configure)
        {
            MainServiceSettings settings = new();
            configure?.Invoke(ref settings);
            services.AddSingleton(settings);

            services.AddScoped<IHarvestService, HarvestService>();
            services.AddHostedService<BusinessLayer.Services.MainService>();

            return services;
        }
    }
}

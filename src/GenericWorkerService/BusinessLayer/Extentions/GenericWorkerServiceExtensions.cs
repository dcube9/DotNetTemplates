using GenericWorkerService.BusinessLayer.Services;
using GenericWorkerService.BusinessLayer.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace GenericWorkerService.BusinessLayer.Extentions
{
    public delegate void ActionRef<T>(ref T item);
    public delegate void ActionOut<T>(out T item);

    public static class GenericWorkerServiceExtensions
    {
        public static IServiceCollection AddGenericWorkerServices(this IServiceCollection services, ActionRef<GenericWorkerSettings> configure)
        {
            GenericWorkerSettings settings = new();
            configure?.Invoke(ref settings);
            services.AddSingleton(settings);

            services.AddScoped<IHarvestService, HarvestService>();
            services.AddHostedService<BusinessLayer.Services.GenericWorkerService>();

            return services;
        }
    }
}

using System;
using GenericWorkerService.BusinessLayer.Services;
using GenericWorkerService.BusinessLayer.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace GenericWorkerService.BusinessLayer.Extentions
{
    public static class WorkerServiceExtensions
    {
        public static IServiceCollection AddGenericWorkerServices(this IServiceCollection services, Action<GenericWorkerSetting> configure)
        {
            GenericWorkerSetting settings = new();
            configure?.Invoke(settings);
            services.AddSingleton(settings);

            services.AddScoped<IHarvestService, HarvestService>();
            services.AddHostedService<BusinessLayer.Services.GenericWorkerService>();

            return services;
        }
    }
}

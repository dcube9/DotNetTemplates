using System;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkerService.BusinessLayer.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericWorkerService.BusinessLayer.Services
{
    public class GenericWorkerService : BackgroundService
    {
        private readonly ILogger<GenericWorkerService> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly GenericWorkerSetting genericWorkerSetting;

        public GenericWorkerService(ILogger<GenericWorkerService> logger,
                             IServiceProvider serviceProvider,
                             GenericWorkerSetting genericWorkerSetting)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.genericWorkerSetting = genericWorkerSetting;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!genericWorkerSetting.IsEnable)
            {
                logger.LogInformation("{worker} is disable.", nameof(GenericWorkerService));
                return;
            }

            logger.LogInformation("{worker} starts.", nameof(GenericWorkerService));
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            logger.LogInformation("{worker} is stopped.", nameof(GenericWorkerService));
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var shouldRunning = true;

            while (shouldRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("{worker} initial delay of {WaitBeforeStart} seconds.", nameof(GenericWorkerService), genericWorkerSetting.WaitBeforeStart / 1000);
                    await Task.Delay(genericWorkerSetting.WaitBeforeStart, cancellationToken);

                    logger.LogInformation("{worker} is running with polling every {PollingFrequency} seconds.", nameof(GenericWorkerService), genericWorkerSetting.PollingFrequency / 1000);

                    using var scope = serviceProvider.CreateScope();

                    var harvestService = scope.ServiceProvider.GetRequiredService<IHarvestService>();
                    await harvestService.HarvestAsync(cancellationToken);

                    //await Task.Delay(genericWorkerSetting.PollingFrequency, cancellationToken);
                }
                catch (Exception e)
                {
                    if (e is not TaskCanceledException)
                    {
                        logger.LogError(e, "{worker} error in ExecuteAsync", nameof(GenericWorkerService));
                    }
                }
                finally
                {
                    logger.LogInformation("{worker} is stopping.", nameof(GenericWorkerService));

                    try
                    {
                        await Task.Delay(genericWorkerSetting.PollingFrequency, cancellationToken);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}

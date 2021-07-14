using System;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkerService.BusinessLayer.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericWorkerService.BusinessLayer.Services
{
    public class MainService : BackgroundService
    {
        private readonly ILogger<MainService> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly MainServiceSettings genericWorkerSetting;

        public MainService(ILogger<MainService> logger,
                             IServiceProvider serviceProvider,
                             MainServiceSettings genericWorkerSetting)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.genericWorkerSetting = genericWorkerSetting;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!genericWorkerSetting.IsEnable)
            {
                logger.LogInformation("{worker} is disable.", nameof(MainService));
                return;
            }

            logger.LogInformation("{worker} starts.", nameof(MainService));
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            logger.LogInformation("{worker} is stopped.", nameof(MainService));
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await ExecuteAndExitOnExceptionAsync(cancellationToken);
        }

        //protected async Task ExecuteTillCancellationAsync(CancellationToken cancellationToken)
        //{
        //    var shouldRunning = true;

        //    try
        //    {
        //        await WaitBeforeStartAsync(cancellationToken);

        //        logger.LogInformation("{worker} is running with polling every {PollingFrequency} seconds.", nameof(MainService),
        //            genericWorkerSetting.PollingFrequency / 1000);
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is not TaskCanceledException)
        //        {
        //            logger.LogError(e, "{worker} error in ExecuteAsync", nameof(GenericWorkerService));
        //        }

        //        logger.LogInformation("{worker} is stopping.", nameof(GenericWorkerService));
        //        return;
        //    }

        //    while (shouldRunning && !cancellationToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            using var scope = serviceProvider.CreateScope();
        //            var harvestService = scope.ServiceProvider.GetRequiredService<IHarvestService>();

        //            await harvestService.HarvestAsync(cancellationToken);
        //        }
        //        catch (Exception e)
        //        {
        //            if (e is not TaskCanceledException)
        //            {
        //                logger.LogError(e, "{worker} error in ExecuteAsync", nameof(GenericWorkerService));
        //            }
        //        }
        //        finally
        //        {
        //            logger.LogInformation("{worker} is stopping.", nameof(GenericWorkerService));

        //            try
        //            {
        //                await Task.Delay(genericWorkerSetting.PollingFrequency, cancellationToken);
        //            }
        //            catch
        //            {
        //                // ignored
        //            }
        //        }
        //    }
        //}

        protected async Task ExecuteAndExitOnExceptionAsync(CancellationToken cancellationToken)
        {
            try
            {
                await WaitBeforeStartAsync(cancellationToken);

                logger.LogInformation("{worker} is running with polling every {PollingFrequency} seconds.", nameof(MainService),
                    genericWorkerSetting.PollingFrequency / 1000);

                while (!cancellationToken.IsCancellationRequested)
                {
                    using var scope = serviceProvider.CreateScope();
                    var harvestService = scope.ServiceProvider.GetRequiredService<IHarvestService>();

                    await harvestService.HarvestAsync(cancellationToken);
                    await Task.Delay(genericWorkerSetting.PollingFrequency, cancellationToken);
                }
            }
            catch (Exception e)
            {
                if (e is not TaskCanceledException)
                {
                    logger.LogError(e, "{worker} error in ExecuteAsync", nameof(MainService));
                }
            }
            finally
            {
                logger.LogInformation("{worker} is stopping.", nameof(MainService));
            }
        }

        private async Task WaitBeforeStartAsync(CancellationToken cancellationToken)
        {
            if (genericWorkerSetting.WaitBeforeStart > 0)
            {
                logger.LogInformation("{worker} initial delay of {WaitBeforeStart} seconds.", nameof(MainService),
                    genericWorkerSetting.WaitBeforeStart / 1000);
                await Task.Delay(genericWorkerSetting.WaitBeforeStart, cancellationToken);
            }
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using GenericWorkerService.BusinessLayer.Settings;
using Microsoft.Extensions.Logging;

namespace GenericWorkerService.BusinessLayer.Services
{
    public class HarvestService : IHarvestService
    {
        private readonly ILogger<HarvestService> logger;
        private readonly GenericWorkerSetting genericWorkerSetting;

        public HarvestService(ILogger<HarvestService> logger, GenericWorkerSetting genericWorkerSetting)
        {
            this.logger = logger;
            this.genericWorkerSetting = genericWorkerSetting;
        }

        public async Task HarvestAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("{worker} Start Harvest at: {time}", nameof(HarvestService), DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken);

            logger.LogInformation("{worker} End Harvest at: {time}", nameof(HarvestService), DateTimeOffset.Now);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

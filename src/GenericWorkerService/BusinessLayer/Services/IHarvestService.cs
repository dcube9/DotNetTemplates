using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericWorkerService.BusinessLayer.Services
{
    public interface IHarvestService : IDisposable
    {
        Task HarvestAsync(CancellationToken stoppingToken);
    }
}
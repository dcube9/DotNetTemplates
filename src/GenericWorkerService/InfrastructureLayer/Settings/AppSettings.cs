using GenericWorkerService.BusinessLayer.Settings;

namespace GenericWorkerService.InfrastructureLayer.Settings
{
    public class AppSettings
    {
        public GenericWorkerSettings GenericWorker { get; set; } = new();
    }
}

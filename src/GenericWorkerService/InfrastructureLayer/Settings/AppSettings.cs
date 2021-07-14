using GenericWorkerService.BusinessLayer.Settings;

namespace GenericWorkerService.InfrastructureLayer.Settings
{
    public class AppSettings
    {
        public MainServiceSettings MainService { get; set; } = new();
    }
}

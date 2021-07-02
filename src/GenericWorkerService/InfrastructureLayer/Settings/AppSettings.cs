namespace GenericWorkerService.InfrastructureLayer.Settings
{
    public class AppSettings
    {
        public bool GenericWorkerIsEnable { get; set; }
        public int GenericWorkerWaitBeforeStart { get; set; } = 5000 * 1;            // Default 5 secondi
        public int GenericWorkerPollingFrequency { get; set; } = 5000 * 1;           // Default 5 secondi
    }
}

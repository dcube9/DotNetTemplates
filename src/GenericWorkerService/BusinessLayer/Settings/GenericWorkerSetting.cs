namespace GenericWorkerService.BusinessLayer.Settings
{
    public class GenericWorkerSetting
    {
        public bool IsEnable { get; set; }
        public int WaitBeforeStart { get; set; } = 5000 * 1;            // Default 5 secondi
        public int PollingFrequency { get; set; } = 5000 * 1;           // Default 5 secondi
    }
}

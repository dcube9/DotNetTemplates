using System;

namespace GenericWorkerService.BusinessLayer.Settings
{
    public class MainServiceSettings : ICloneable
    {
        public bool IsEnable { get; set; } = true;

        public int WaitBeforeStart { get; set; } = 60 * 1000;           // Default 1 minuto

        public int PollingFrequency { get; set; } = 60 * 1000;          // Default 1 minuto

        public object Clone() => MemberwiseClone();
    }
}

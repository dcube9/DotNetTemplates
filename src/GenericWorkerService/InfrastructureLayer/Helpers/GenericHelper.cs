using System;
using System.IO;
using System.Reflection;

namespace GenericWorkerService.InfrastructureLayer.Helpers
{
    public class GenericHelper
    {
        public static string ExecutingAssemblyFolder
        {
            get
            {
                var location = Assembly.GetExecutingAssembly().Location;
                UriBuilder uri = new(location);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}

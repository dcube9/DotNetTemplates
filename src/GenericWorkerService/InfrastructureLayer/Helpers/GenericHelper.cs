using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GenericWorkerService.InfrastructureLayer.Helpers
{
    public class AssemblyHelpers
    {
        public static string ExecutingFolder
        {
            get
            {
                var location = Assembly.GetExecutingAssembly().Location;
                UriBuilder uri = new(location);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string AssemblyVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return string.IsNullOrEmpty(fvi.FileVersion) ? "" : fvi.FileVersion;
        }
    }
}

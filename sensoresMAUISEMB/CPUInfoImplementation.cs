using System;
using System.Management;
using System.Threading.Tasks;

namespace sensoresMAUISEMB
{
    public class CPUInfoImplementation : ICPUInfo
    {
        public Task<string> GetCPUInfoAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    // Use ManagementObjectSearcher to get CPU information
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                    {
                        using (var objects = searcher.Get())
                        {
                            // Use a loop to access the objects
                            foreach (ManagementObject cpuObject in objects)
                            {
                                var cpuUsage = cpuObject["LoadPercentage"]?.ToString();
                                var coreCount = cpuObject["NumberOfCores"]?.ToString();
                                var cpuFrequency = cpuObject["CurrentClockSpeed"]?.ToString();

                                return $"CPU Usage: {cpuUsage}%\nNumber of Cores: {coreCount}\nCPU Frequency: {cpuFrequency} MHz";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions gracefully
                    return $"Failed to retrieve CPU information: {ex.Message}";
                }

                return "No CPU information available.";
            });
        }
    }
}

using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace sensoresMAUISEMB
{
    public partial class Performance : ContentPage
    {
        public Performance()
        {
            InitializeComponent();
            GetCPUInfo();
            StartCPUUsageUpdates();
        }

        private void GetCPUInfo()
        {
#if ANDROID
            try
            {
                Java.Lang.Runtime runtime = Java.Lang.Runtime.GetRuntime();
                int coreCount = runtime.AvailableProcessors();
                coreCountLabel.Text = $"Cores: {coreCount}";

                string cpuFrequencies = string.Empty;

                for (int i = 0; i < coreCount; i++)
                {
                    string path = $"/sys/devices/system/cpu/cpu{i}/cpufreq/scaling_cur_freq";
                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            // Read the frequency in kHz
                            string freqStr = System.IO.File.ReadAllText(path).Trim();
                            long freqKHz = long.Parse(freqStr);

                            // Convert to MHz
                            double freqMHz = freqKHz / 1000.0;
                            cpuFrequencies += $"Core {i}: {freqMHz} MHz\n";
                        }
                        catch (Exception ex)
                        {
                            cpuFrequencies += $"Core {i}: Error reading frequency - {ex.Message}\n";
                            Debug.WriteLine($"Error reading frequency for core {i}: {ex}");
                        }
                    }
                    else
                    {
                        cpuFrequencies += $"Core {i}: Frequency info not available\n";
                    }
                }

                cpuFrequencyLabel.Text = cpuFrequencies;
            }
            catch (Exception ex)
            {
                coreCountLabel.Text = $"Cores: Error - {ex.Message}";
                cpuFrequencyLabel.Text = $"Frequency: Error - {ex.Message}";
                Debug.WriteLine($"Error getting CPU info: {ex}");
            }
#else
    coreCountLabel.Text = "Cores: Not Supported on this platform";
    cpuFrequencyLabel.Text = "Frequency: Not Supported on this platform";
#endif
        }


        private async void StartCPUUsageUpdates()
        {
            while (true)
            {
                try
                {
#if ANDROID
                    cpuUsageLabel.Text = $"CPU Usage: {await GetCPUUsageAsync()}%";
#else
                    cpuUsageLabel.Text = "CPU Usage: Not Supported on this platform";
#endif
                }
                catch (Exception ex)
                {
                    cpuUsageLabel.Text = $"CPU Usage: Error - {ex.Message}";
                    Debug.WriteLine($"Error getting CPU usage: {ex}");
                }
                await Task.Delay(1000);
            }
        }



        private async Task<double> GetCPUUsageAsync()
        {
#if ANDROID
            try
            {
                long startIdle = getDeviceIdleTime();
                long startTotal = getDeviceUptime();

                await Task.Delay(100);

                long endIdle = getDeviceIdleTime();
                long endTotal = getDeviceUptime();


                double idleTime = endIdle - startIdle;
                double totalTime = endTotal - startTotal;
                double cpuUsage = (1.0 - (idleTime / totalTime)) * 100;

                return Math.Round(cpuUsage, 2);

            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Error getting CPU Usage: {ex}");
                return 0;
            }
#else
            return 0;
#endif
        }


#if ANDROID

        private long getDeviceUptime()
        {
            return Android.OS.SystemClock.ElapsedRealtime();
        }


        private long getDeviceIdleTime()
        {

            return Android.OS.SystemClock.CurrentThreadTimeMillis();
        }
#endif
    }
}
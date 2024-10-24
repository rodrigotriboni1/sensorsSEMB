using Microsoft.Maui.Controls;
using sensoresMAUISEMB.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace sensoresMAUISEMB
{

    public partial class Performance : ContentPage
    {
        public ObservableCollection<CPUUsageData> CPUUsageData { get; set; }

        public ObservableCollection<CoreInfo> CoresList { get; set; }

        public Performance()
        {
            InitializeComponent();
            CoresList = new ObservableCollection<CoreInfo>();
            CPUUsageData = new ObservableCollection<CPUUsageData>();
            BindingContext = this;
            GetCPUInfo();
            StartCPUUsageAndFrequencyUpdates();
        }

        private void GetCPUInfo()
        {
#if ANDROID
            try
            {
                Java.Lang.Runtime runtime = Java.Lang.Runtime.GetRuntime();
                int coreCount = runtime.AvailableProcessors();
                coreCountLabel.Text = $"Cores: {coreCount}";
            }
            catch (Exception ex)
            {
                coreCountLabel.Text = $"Cores: Error - {ex.Message}";
                Debug.WriteLine($"Error getting CPU info: {ex}");
            }
#else
            coreCountLabel.Text = "Cores: Not Supported on this platform";
#endif
        }

        private async void StartCPUUsageAndFrequencyUpdates()
        {
            int timeCounter = 0; // To track the time for the chart
            while (true)
            {
                try
                {
#if ANDROID
                    // Get CPU Usage
                    double cpuUsage = await GetCPUUsageAsync();
                    cpuUsageLabel.Text = $"CPU Usage: {cpuUsage}%";

                    // Add data to the chart
                    CPUUsageData.Add(new CPUUsageData { Time = timeCounter++, Usage = cpuUsage });

                    // Get CPU Frequencies
                    await GetCPUFrequenciesAsync();
#else
                cpuUsageLabel.Text = "CPU Usage: Not Supported on this platform";
#endif
                }
                catch (Exception ex)
                {
                    cpuUsageLabel.Text = $"CPU Usage: Error - {ex.Message}";
                    Debug.WriteLine($"Error getting CPU usage or frequencies: {ex}");
                }
                await Task.Delay(100); // Update milisecond
            }
        }

        private async Task GetCPUFrequenciesAsync()
        {
#if ANDROID
            try
            {
                Java.Lang.Runtime runtime = Java.Lang.Runtime.GetRuntime();
                int coreCount = runtime.AvailableProcessors();
                CoresList.Clear();

                for (int i = 0; i < coreCount; i++)
                {
                    string path = $"/sys/devices/system/cpu/cpu{i}/cpufreq/scaling_cur_freq";
                    if (File.Exists(path))
                    {
                        try
                        {
                            // Read the frequency in kHz
                            string freqStr = await File.ReadAllTextAsync(path);
                            long freqKHz = long.Parse(freqStr.Trim());

                            // Convert to MHz
                            double freqMHz = freqKHz / 1000.0;
                            CoresList.Add(new CoreInfo
                            {
                                CoreName = $"Core {i}",
                                Frequency = $"{freqMHz} MHz"
                            });
                        }
                        catch (Exception ex)
                        {
                            CoresList.Add(new CoreInfo
                            {
                                CoreName = $"Core {i}",
                                Frequency = $"Error reading frequency - {ex.Message}"
                            });
                            Debug.WriteLine($"Error reading frequency for core {i}: {ex}");
                        }
                    }
                    else
                    {
                        CoresList.Add(new CoreInfo
                        {
                            CoreName = $"Core {i}",
                            Frequency = "Frequency info not available"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting CPU frequencies: {ex}");
            }
#endif
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
                Debug.WriteLine($"Error getting CPU usage: {ex}");
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

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace sensoresMAUISEMB
{
    public partial class Performance : ContentPage
    {
        public Performance()
        {
            InitializeComponent();
            LoadPerformanceData();
        }

        private async void LoadPerformanceData()
        {
            try
            {
                // Use the dependency service to get CPU information
                var cpuInfoService = DependencyService.Get<ICPUInfo>();
                var cpuInfo = await cpuInfoService.GetCPUInfoAsync();

                // Display the CPU info in a Label
                cpuInfoLabel.Text = cpuInfo; // Assuming you have a Label named cpuInfoLabel
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully
                await DisplayAlert("Error", $"Failed to retrieve CPU information: {ex.Message}", "OK");
            }
        }
    }
}
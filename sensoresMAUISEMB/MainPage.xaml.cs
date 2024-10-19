using Syncfusion.Maui.Charts;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
namespace sensoresMAUISEMB
{
    public partial class MainPage : ContentPage
    {
        private SensorManager sensorManager;
        private SensorChartManager sensorChartManager;
        public MainPage()
        {
            InitializeComponent();

            sensorManager = new SensorManager(
                AccelLabel,
                BarometerLabel,
                CompassLabel,
                GyroscopeLabel,
                MagnetometerLabel,
                OrientationLabel);

            // Inicializa o sensorChartManager passando o gráfico
            sensorChartManager = new SensorChartManager(sensorChart);
        }

        private void OnToggleAccelerometerClicked(object sender, EventArgs e)
        {
            sensorManager.AccelerometerReadingChanged += OnAccelerometerReadingChanged;
            sensorManager.ToggleAccelerometer();
        }

        private void OnToggleBarometerClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleBarometer();
        }

        private void OnToggleCompassClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleCompass();
        }

        private void OnToggleGyroscopeClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleGyroscope();
        }

        private void OnToggleMagnetometerClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleMagnetometer();
        }

        private void OnToggleOrientationClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleOrientation();
        }

        private void OnToggleShakeClicked(object sender, EventArgs e)
        {
            sensorManager.ToggleShake();
        }

        private void OnAccelerometerReadingChanged(double x, double y, double z)
        {
            sensorChartManager.AddAccelerometerReading(x, y, z);
        }

        private void OnToggleGraphClicked(object sender, EventArgs e)
        {
            sensorChart.IsVisible = !sensorChart.IsVisible;
        }
    }
}

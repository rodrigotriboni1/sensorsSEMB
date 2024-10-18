using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace sensoresMAUISEMB
{
    public partial class MainPage : ContentPage
    {
        private SensorManager sensorManager;
        public ObservableCollection<ISeries> SensorSeries { get; set; }
        private int maxPoints = 70;
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

            SensorSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<double> { Name = "X Axis", 
                    GeometrySize = 0,
                    LineSmoothness = 0,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Values = new ObservableCollection<double>() },
                new LineSeries<double> { Name = "Y Axis",
                    GeometrySize = 0,
                    LineSmoothness = 0,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Values = new ObservableCollection<double>() },
                new LineSeries<double> { Name = "Z Axis", 
                    GeometrySize = 0,
                    LineSmoothness = 0,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Values = new ObservableCollection<double>() }
            };
            sensorChart.Series = SensorSeries;

            //sensorManager.ToggleAccelerometer();
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

            var xValues = SensorSeries[0].Values as ObservableCollection<double>;
            var yValues = SensorSeries[1].Values as ObservableCollection<double>;
            var zValues = SensorSeries[2].Values as ObservableCollection<double>;

            if (xValues != null) xValues.Add(x);
            if (yValues != null) yValues.Add(y);
            if (zValues != null) zValues.Add(z);

            if (xValues?.Count > maxPoints) xValues.RemoveAt(0);
            if (yValues?.Count > maxPoints) yValues.RemoveAt(0);
            if (zValues?.Count > maxPoints) zValues.RemoveAt(0);
        }

        private void OnToggleGraphClicked(object sender, EventArgs e)
        {
            // Toggle the visibility of the chart
            sensorChart.IsVisible = !sensorChart.IsVisible;
        }

        private void OnOtherActionClicked(object sender, EventArgs e)
        {
            // Implement your other action here
        }

    }
}

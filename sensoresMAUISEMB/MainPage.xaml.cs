using OxyPlot.Series;
using OxyPlot;
using SkiaSharp;
using System.Collections.Generic; // Ensure you include this
using OxyPlot.Axes;
using OxyPlot.Maui.Skia;

namespace sensoresMAUISEMB
{
    public partial class MainPage : ContentPage
    {
        private SensorManager sensorManager;
        private PlotModel _plotModel;
        private const int MaxPoints = 100; // Maximum number of points to display 
        private Queue<DataPoint> _dataPointsX = new Queue<DataPoint>(); // For X values
        private Queue<DataPoint> _dataPointsY = new Queue<DataPoint>(); // For Y values
        private Queue<DataPoint> _dataPointsZ = new Queue<DataPoint>(); // For Z values

        public MainPage()
        {
            InitializeComponent();

            InitializePlotModel(); // Initialize the plot model

            sensorManager = new SensorManager(
                AccelLabel,
                BarometerLabel,
                CompassLabel,
                GyroscopeLabel,
                MagnetometerLabel,
                OrientationLabel,
                UpdatePlot);

            sensorManager.SetPlotModel(_plotModel); // Pass the PlotModel to SensorManager
        }

        private void OnToggleAccelerometerClicked(object sender, EventArgs e)
        {
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


        private void InitializePlotModel()
        {
            _plotModel = new PlotModel
            {
                PlotMargins = new OxyThickness(0),
            };

            // Configure axes
            var leftAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Sensor Value",
            };

            var bottomAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Data Points",
                Maximum = MaxPoints // Maximum number of points to display
            };

            _plotModel.Axes.Add(leftAxis);
            _plotModel.Axes.Add(bottomAxis);

            // Add series for each axis
            _plotModel.Series.Add(new LineSeries { Title = "X Axis", Color = OxyColors.Red });
            _plotModel.Series.Add(new LineSeries { Title = "Y Axis", Color = OxyColors.Green });
            _plotModel.Series.Add(new LineSeries { Title = "Z Axis", Color = OxyColors.Blue });

            plotView.Model = _plotModel;
        }

        public void UpdatePlot(double x, double y, double z)
        {
            // Update data points for each axis
            UpdateDataPoint(_dataPointsX, x);
            UpdateDataPoint(_dataPointsY, y);
            UpdateDataPoint(_dataPointsZ, z);

            // Update the plot with the new data
            RefreshPlot();
        }

        private void UpdateDataPoint(Queue<DataPoint> dataPoints, double value)
        {
            if (dataPoints.Count >= MaxPoints)
            {
                dataPoints.Dequeue(); // Remove the oldest point if we already have MaxPoints
            }

            // Enqueue the new data point
            dataPoints.Enqueue(new DataPoint(dataPoints.Count, value)); // X is the current index, Y is the sensor value
        }

        private void RefreshPlot()
        {
            // Clear previous series data
            var xSeries = _plotModel.Series[0] as LineSeries;
            var ySeries = _plotModel.Series[1] as LineSeries;
            var zSeries = _plotModel.Series[2] as LineSeries;

            xSeries.Points.Clear();
            ySeries.Points.Clear();
            zSeries.Points.Clear();

            // Add current data points to the series
            foreach (var point in _dataPointsX)
            {
                xSeries.Points.Add(point);
            }
            foreach (var point in _dataPointsY)
            {
                ySeries.Points.Add(point);
            }
            foreach (var point in _dataPointsZ)
            {
                zSeries.Points.Add(point);
            }

            // Notify that the model has changed
            _plotModel.InvalidatePlot(true);
        }
    }
}

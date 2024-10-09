using SkiaSharp;
using System.Collections.Generic;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Maui.Skia;
using OxyPlot.Axes;


namespace sensoresMAUISEMB
{
    class SensorManager
    {
        Label AccelLabel, BarometerLabel, CompassLabel, GyroscopeLabel, MagnetometerLabel, OrientationLabel;
        Label ShakeLabel; // Label para mostrar a detecção de shake

        public delegate void UpdatePlotDelegate(double x, double y, double z);
        private UpdatePlotDelegate _updatePlot;

        private PlotModel _plotModel;
        private LineSeries _xAxisSeries, _yAxisSeries, _zAxisSeries;
        private int _dataPointCount = 0;

        public bool IsAccelerometerMonitoring => Accelerometer.Default.IsMonitoring;

        public SensorManager(Label accelLabel, Label barometerLabel, Label compassLabel,
                             Label gyroscopeLabel, Label magnetometerLabel, Label orientationLabel,       
                             UpdatePlotDelegate updatePlot)
        {
            AccelLabel = accelLabel;
            BarometerLabel = barometerLabel;
            CompassLabel = compassLabel;
            GyroscopeLabel = gyroscopeLabel;
            MagnetometerLabel = magnetometerLabel;
            OrientationLabel = orientationLabel;
            _updatePlot = updatePlot;
        }

        public void SetPlotModel(PlotModel plotModel)
        {
            _plotModel = plotModel;
            InitializePlot(); // Inicializa as séries do gráfico usando o modelo
        }

        private void InitializePlot()
        {
            // Verifica se _plotModel é nulo para evitar NullReferenceException
            if (_plotModel == null)
            {
                throw new InvalidOperationException("O modelo do gráfico deve ser definido antes da inicialização do gráfico.");
            }

            _xAxisSeries = new LineSeries { Title = "Eixo X", Color = OxyColors.Red };
            _yAxisSeries = new LineSeries { Title = "Eixo Y", Color = OxyColors.Green };
            _zAxisSeries = new LineSeries { Title = "Eixo Z", Color = OxyColors.Blue };

            _plotModel.Series.Add(_xAxisSeries);
            _plotModel.Series.Add(_yAxisSeries);
            _plotModel.Series.Add(_zAxisSeries);
        }

        public void UpdatePlot(double x, double y, double z)
        {
            _xAxisSeries.Points.Add(new DataPoint(_dataPointCount, x));
            _yAxisSeries.Points.Add(new DataPoint(_dataPointCount, y));
            _zAxisSeries.Points.Add(new DataPoint(_dataPointCount, z));

            _dataPointCount++;
            _plotModel.InvalidatePlot(true); // Atualiza o gráfico para mostrar os novos dados
        }

        private void ClearPlotData()
        {
            _xAxisSeries.Points.Clear();
            _yAxisSeries.Points.Clear();
            _zAxisSeries.Points.Clear();
            _dataPointCount = 0; // Reseta o contador de pontos
        }

        private async void ShowSensorNotSupportedMessage(string sensorName)
        {
            string message = $"{sensorName} não é compatível com o celular.";

            // Exibe a mensagem no AlertBox
            await DisplayAlert("Sensor não suportado", message, "OK");
        }

        private async Task DisplayAlert(string title, string message, string cancel)
        {
            await App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public void ToggleAccelerometer()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    Console.WriteLine("Acelerômetro: Iniciando");
                    Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Console.WriteLine("Acelerômetro: Parando");
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
                    ClearPlotData(); // Limpa os dados do gráfico
                }
            }
            else
            {
                ShowSensorNotSupportedMessage("Acelerômetro");
                Console.WriteLine("Acelerômetro: Não Suportado");
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            Console.WriteLine($"Acelerômetro: Leitura = {e.Reading}");

            if (AccelLabel != null)
            {
                AccelLabel.TextColor = Colors.Green;
                

                var data = e.Reading;
                double x = data.Acceleration.X;
                double y = data.Acceleration.Y;
                double z = data.Acceleration.Z;

                // Inicializa o modelo do gráfico quando o acelerômetro começa
                if (_plotModel == null)
                {
                    InitializePlot(); // Inicializa o gráfico aqui
                }

                // Atualiza o gráfico com novos dados
                UpdatePlot(x, y, z);
            }
            else
            {
                Console.WriteLine("Acelerômetro: AccelLabel é nulo!");
            }
        }

        public void ToggleShake()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    // Ativa o acelerômetro para detecção de shake
                    Accelerometer.Default.ShakeDetected += Accelerometer_ShakeDetected;
                    Accelerometer.Default.Start(SensorSpeed.Game);
                }
                else
                {
                    // Desativa o acelerômetro
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ShakeDetected -= Accelerometer_ShakeDetected;
                }
            }
        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Atualiza o UI Label com uma mensagem de "shake detected", em uma cor aleatória
            ShakeLabel.TextColor = new Color(Random.Shared.Next(256), Random.Shared.Next(256), Random.Shared.Next(256));
            ShakeLabel.Text = $"Shake detected";
        }

    public void ToggleBarometer()
        {
            if (Barometer.Default.IsSupported)
            {
                if (!Barometer.Default.IsMonitoring)
                {
                    Console.WriteLine("Barometer: Starting");
                    Barometer.Default.ReadingChanged += Barometer_ReadingChanged;
                    Barometer.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Console.WriteLine("Barometer: Stopping");
                    Barometer.Default.Stop();
                    Barometer.Default.ReadingChanged -= Barometer_ReadingChanged;
                }
            }
            else
            {
                ShowSensorNotSupportedMessage("Barômetro");
                Console.WriteLine("Barometer: Not Supported");
            }
        }

        private void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            Console.WriteLine($"Barometer: Reading = {e.Reading}");
            if (BarometerLabel != null)
            {
                BarometerLabel.TextColor = Colors.Green;
                BarometerLabel.Text = $"Barometer: {e.Reading:F2} hPa";
            }
            else
            {
                Console.WriteLine("Barometer: BarometerLabel is null!");
            }
        }

        public void ToggleCompass()
        {
            if (Compass.Default.IsSupported)
            {
                if (!Compass.Default.IsMonitoring)
                {
                    Console.WriteLine("Compass: Starting");
                    Compass.Default.ReadingChanged += Compass_ReadingChanged;
                    Compass.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Console.WriteLine("Compass: Stopping");
                    Compass.Default.Stop();
                    Compass.Default.ReadingChanged -= Compass_ReadingChanged;
                }
            }
            else
            {
                ShowSensorNotSupportedMessage("Compasso");
                Console.WriteLine("Compass: Not Supported");
            }
        }

        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            Console.WriteLine($"Compass: Reading = {e.Reading}");
            if (CompassLabel != null)
            {
                CompassLabel.TextColor = Colors.Green;
                CompassLabel.Text = $"Compass: {e.Reading:F2} degrees";
            }
            else
            {
                Console.WriteLine("Compass: CompassLabel is null!");
            }
        }

       

        public void ToggleGyroscope()
        {
            if (Gyroscope.Default.IsSupported)
            {
                if (!Gyroscope.Default.IsMonitoring)
                {
                    Console.WriteLine("Gyroscope: Starting");
                    Gyroscope.Default.ReadingChanged += Gyroscope_ReadingChanged;
                    Gyroscope.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Console.WriteLine("Gyroscope: Stopping");
                    Gyroscope.Default.Stop();
                    Gyroscope.Default.ReadingChanged -= Gyroscope_ReadingChanged;
                }
            }
            else
            {
                ShowSensorNotSupportedMessage("Giroscópio");
                Console.WriteLine("Gyroscope: Not Supported");
            }
        }

        private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            Console.WriteLine($"Gyroscope: Reading = {e.Reading}");
            if (GyroscopeLabel != null)
            {
                GyroscopeLabel.TextColor = Colors.Green;
                GyroscopeLabel.Text = $"Gyroscope: {e.Reading}";
            }
            else
            {
                Console.WriteLine("Gyroscope: GyroscopeLabel is null!");
            }
        }

        public void ToggleMagnetometer()
        {
            if (Magnetometer.Default.IsSupported)
            {
                if (!Magnetometer.Default.IsMonitoring)
                {
                    Console.WriteLine("Magnetometer: Starting");
                    Magnetometer.Default.ReadingChanged += Magnetometer_ReadingChanged;
                    Magnetometer.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Console.WriteLine("Magnetometer: Stopping");
                    Magnetometer.Default.Stop();
                    Magnetometer.Default.ReadingChanged -= Magnetometer_ReadingChanged;
                }
            }
            else
            {
                ShowSensorNotSupportedMessage("Magnetômetro");
                Console.WriteLine("Magnetometer: Not Supported");
            }
        }

        private void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            Console.WriteLine($"Magnetometer: Reading = {e.Reading}");
            if (MagnetometerLabel != null)
            {
                MagnetometerLabel.TextColor = Colors.Green;
                MagnetometerLabel.Text = $"Magnetometer: {e.Reading}";
            }
            else
            {
                Console.WriteLine("Magnetometer: MagnetometerLabel is null!");
            }
        }

        public void ToggleOrientation()
        {
            if (OrientationSensor.Default.IsSupported)
            {
                if (!OrientationSensor.Default.IsMonitoring)
                {
                    Console.WriteLine("Orientation: Starting");
                    OrientationSensor.Default.ReadingChanged += Orientation_ReadingChanged;
                    OrientationSensor.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Console.WriteLine("Orientation: Stopping");
                    OrientationSensor.Default.Stop();
                    OrientationSensor.Default.ReadingChanged -= Orientation_ReadingChanged;
                }
            }
            else
            {
                ShowSensorNotSupportedMessage("Sensor de Orientação");
                Console.WriteLine("Orientation: Not Supported");
            }
        }

        private void Orientation_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            Console.WriteLine($"Orientation: Reading = {e.Reading}");
            if (OrientationLabel != null)
            {
                OrientationLabel.TextColor = Colors.Green;
                OrientationLabel.Text = $"Orientation: {e.Reading}";
            }
            else
            {
                Console.WriteLine("Orientation: OrientationLabel is null!");
            }
        }
    }
}
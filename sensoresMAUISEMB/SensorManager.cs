using SkiaSharp;
using System.Collections.Generic;


namespace sensoresMAUISEMB
{
    class SensorManager
    {
        Label AccelLabel, BarometerLabel, CompassLabel, GyroscopeLabel, MagnetometerLabel, OrientationLabel, ShakeLabel;

        public event Action<double, double, double> AccelerometerReadingChanged;

        public SensorManager(Label accelLabel, Label barometerLabel, Label compassLabel,
                             Label gyroscopeLabel, Label magnetometerLabel, Label orientationLabel)
        {
            AccelLabel = accelLabel;
            BarometerLabel = barometerLabel;
            CompassLabel = compassLabel;
            GyroscopeLabel = gyroscopeLabel;
            MagnetometerLabel = magnetometerLabel;
            OrientationLabel = orientationLabel;
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
                    Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
                    Accelerometer.Default.Stop();
                }
            }
            else
            {
                Console.WriteLine("Acelerômetro: Não Suportado");
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            double x = data.Acceleration.X;
            double y = data.Acceleration.Y;
            double z = data.Acceleration.Z;

            AccelLabel.Text = $"X: {x:F4}, Y: {y:F4}, Z: {z:F4}";
            AccelLabel.TextColor = Colors.Green;

            AccelerometerReadingChanged?.Invoke(x, y, z);
        }
        public void ToggleShake()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    Accelerometer.Default.ShakeDetected += Accelerometer_ShakeDetected;
                    Accelerometer.Default.Start(SensorSpeed.Game);
                }
                else
                {
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
                SensorUtils.ShowSensorNotSupportedMessage("Barômetro");
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
                SensorUtils.ShowSensorNotSupportedMessage("Compass");
                Console.WriteLine("Compass: Not Supported");
            }
        }

        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            Console.WriteLine($"Compass: Reading = {e.Reading}");
            if (CompassLabel != null)
            {
                CompassLabel.TextColor = Colors.Green;
                CompassLabel.Text = $"{e.Reading:F2} degrees";
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
                SensorUtils.ShowSensorNotSupportedMessage("Giroscópio");
                Console.WriteLine("Gyroscope: Not Supported");
            }
        }

        private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            Console.WriteLine($"Gyroscope: Reading = {e.Reading}");

            var reading = e.Reading;
            // Usa UpdateSensorLabel para lidar com os três componentes do giroscópio (X, Y, Z)
            SensorUtils.UpdateSensorLabel(GyroscopeLabel, "°/s", reading.AngularVelocity.X, reading.AngularVelocity.Y, reading.AngularVelocity.Z);
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
                SensorUtils.ShowSensorNotSupportedMessage("Magnetômetro");
                Console.WriteLine("Magnetometer: Not Supported");
            }
        }

        private void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            Console.WriteLine($"Magnetometer: Reading = {e.Reading}");

            var reading = e.Reading;
            // Usa UpdateSensorLabel para lidar com os três componentes do magnetômetro (X, Y, Z)
            SensorUtils.UpdateSensorLabel(MagnetometerLabel, "µT", reading.MagneticField.X, reading.MagneticField.Y, reading.MagneticField.Z);
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
                SensorUtils.ShowSensorNotSupportedMessage("Sensor de Orientação");
                Console.WriteLine("Orientation: Not Supported");
            }
        }

        private void Orientation_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            Console.WriteLine($"Orientation: Reading = {e.Reading}");

            var reading = e.Reading;
            SensorUtils.UpdateSensorLabel(OrientationLabel, "units", reading.Orientation.X, reading.Orientation.Y, reading.Orientation.Z, reading.Orientation.W);
        }
    }
}
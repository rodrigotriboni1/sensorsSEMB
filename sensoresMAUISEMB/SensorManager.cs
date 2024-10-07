using Microsoft.Maui.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sensoresMAUISEMB
{
    class SensorManager
    {
        Label AccelLabel, BarometerLabel, CompassLabel, GyroscopeLabel, MagnetometerLabel, OrientationLabel, ShakeLabel;

        public SensorManager(Label accelLabel, Label barometerLabel, Label compassLabel, Label gyroscopeLabel, Label magnetometerLabel, Label orientationLabel)
        {
            AccelLabel = accelLabel;
            BarometerLabel = barometerLabel;
            CompassLabel = compassLabel;
            GyroscopeLabel = gyroscopeLabel;
            MagnetometerLabel = magnetometerLabel;
            OrientationLabel = orientationLabel;
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
                    Console.WriteLine("Accelerometer: Starting");
                    Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Console.WriteLine("Accelerometer: Stopping");
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
                }
            }
            else
            {
                ShowSensorNotSupportedMessage("Acelerômetro");
                Console.WriteLine("Accelerometer: Not Supported");
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            Console.WriteLine($"Accelerometer: Reading = {e.Reading}");
            if (AccelLabel != null)
            {
                AccelLabel.TextColor = Colors.Green;
                AccelLabel.Text = $"Accel: {e.Reading}";
            }
            else
            {
                Console.WriteLine("Accelerometer: AccelLabel is null!");
            }
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

        public void ToggleShake()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    // Turn on accelerometer
                    Accelerometer.Default.ShakeDetected += Accelerometer_ShakeDetected;
                    Accelerometer.Default.Start(SensorSpeed.Game);
                }
                else
                {
                    // Turn off accelerometer
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ShakeDetected -= Accelerometer_ShakeDetected;
                }
            }
        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Update UI Label with a "shaked detected" notice, in a randomized color
            ShakeLabel.TextColor = new Color(Random.Shared.Next(256), Random.Shared.Next(256), Random.Shared.Next(256));
            ShakeLabel.Text = $"Shake detected";
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
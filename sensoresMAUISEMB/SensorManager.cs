using SkiaSharp;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.Maui.Controls.PlatformConfiguration;


namespace sensoresMAUISEMB
{
    internal class SensorManager(Label accelLabel, Label barometerLabel, Label compassLabel,
                         Label gyroscopeLabel, Label magnetometerLabel, Label orientationLabel)
    {
        readonly Label AccelLabel = accelLabel, BarometerLabel = barometerLabel, CompassLabel = compassLabel, GyroscopeLabel = gyroscopeLabel, MagnetometerLabel = magnetometerLabel, OrientationLabel = orientationLabel;

        public event Action<double, double, double>? AccelerometerReadingChanged;
       // public event EventHandler<CompassChangedEventArgs>? CompassReadingChanged;

        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "http://192.168.100.28:5202/api/SensorData"; // API route

        private SensorData CreateSensorData(string sensorName, string sensorType, double valueX, double valueY, double valueZ)
        {
            return new SensorData
            {
                SensorName = sensorName,
                SensorType = sensorType,
                ValueX = valueX,
                ValueY = valueY,
                ValueZ = valueZ,
                Timestamp = DateTime.UtcNow // Or DateTime.Now if you prefer local time
            };
        }
        private bool IsNetworkAvailable()
        {

            return true;
        }
        public async Task SendSensorDataAsync(SensorData sensorData)
        {
            try
            {
                // Check if the Android device/emulator is connected to the internet
                if (!IsNetworkAvailable())
                {
                    Console.WriteLine("Network is unavailable. Please check your connection.");
                    return;
                }

                // Serialize the sensor data to JSON
                var json = JsonSerializer.Serialize(sensorData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the HTTP POST request
                var response = await _httpClient.PostAsync(ApiUrl, content);

                // Handle the response
                if (response.IsSuccessStatusCode)
                {
                    var returnedData = await response.Content.ReadFromJsonAsync<SensorData>();
                    if (returnedData != null)
                    {
                        sensorData.Id = returnedData.Id; // Update the ID
                        Console.WriteLine($"Data sent successfully! ID: {sensorData.Id}");
                    }
                    else
                    {
                        Console.WriteLine("Response was successful, but no ID returned.");
                    }
                }
                else
                {
                    // Capture the error content and status code
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");


                    // You could also log the error to an external logging service if needed.
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Error: {httpEx.Message}");
            }
            catch (TaskCanceledException ex)
            {
                if (!IsNetworkAvailable())
                {
                    Console.WriteLine("Request was canceled due to a network issue. Please check your connection.");
                }
         
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
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
                Task.Run(async () => await SensorUtils.ShowSensorNotSupportedMessage("Acelerômetro"));

                Console.WriteLine("Acelerômetro: Não Suportado");

            }
        }

        private async void Accelerometer_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            double x = data.Acceleration.X;
            double y = data.Acceleration.Y;
            double z = data.Acceleration.Z;

            AccelLabel.Text = $"X: {x:F4}, Y: {y:F4}, Z: {z:F4}";
            AccelLabel.TextColor = Colors.Green;
            
            var sensorData = CreateSensorData("Accelerometer", "Motion", x, y, z);
            Console.WriteLine(sensorData);
            await SendSensorDataAsync(sensorData); 

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
            else
            {
                Task.Run(async () => await SensorUtils.ShowSensorNotSupportedMessage("Acelerômetro"));
                Console.WriteLine("Acelerômetro: Not Supported");
            }
        }

        private void Accelerometer_ShakeDetected(object? sender, EventArgs e)
        {
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
                Task.Run(async () => await SensorUtils.ShowSensorNotSupportedMessage("Barômetro"));
                Console.WriteLine("Barometer: Not Supported");
            }
        }

        private void Barometer_ReadingChanged(object? sender, BarometerChangedEventArgs e)
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
                Task.Run(async () => await SensorUtils.ShowSensorNotSupportedMessage("Compass"));
                Console.WriteLine("Compass: Not Supported");
            }
        }

        private void Compass_ReadingChanged(object? sender, CompassChangedEventArgs e)
        {
            Console.WriteLine($"Compass: Reading = {e.Reading.HeadingMagneticNorth}");
            if (CompassLabel != null)
            {
                CompassLabel.TextColor = Colors.Green;
                int roundedReading = (int)Math.Round(e.Reading.HeadingMagneticNorth); 
                CompassLabel.Text = $"{roundedReading} degrees";
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
                Task.Run(async () => await SensorUtils.ShowSensorNotSupportedMessage("Giroscópio"));
                Console.WriteLine("Gyroscope: Not Supported");
            }
        }

        private void Gyroscope_ReadingChanged(object? sender, GyroscopeChangedEventArgs e)
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
                Task.Run(async () => await SensorUtils.ShowSensorNotSupportedMessage("Magnetômetro"));
                Console.WriteLine("Magnetometer: Not Supported");
            }
        }

        private void Magnetometer_ReadingChanged(object? sender, MagnetometerChangedEventArgs e)
        {
            Console.WriteLine($"Magnetometer: Reading = {e.Reading}");

            var reading = e.Reading;
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
                Task.Run(async () => await SensorUtils.ShowSensorNotSupportedMessage("Sensor de Orientação"));
                Console.WriteLine("Orientation: Not Supported");
            }
        }

        private void Orientation_ReadingChanged(object? sender, OrientationSensorChangedEventArgs e)
        {
            Console.WriteLine($"Orientation: Reading = {e.Reading}");

            var reading = e.Reading;
            SensorUtils.UpdateSensorLabel(OrientationLabel, "units", reading.Orientation.X, reading.Orientation.Y, reading.Orientation.Z, reading.Orientation.W);
        }
    }
}
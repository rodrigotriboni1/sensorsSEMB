using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace sensoresMAUISEMB
{
   
    public static class SensorUtils
    {
        public static void UpdateSensorLabel(Label label, string unit, params double[] readings)
        {
            if (label != null)
            {
                label.TextColor = Colors.Green;

                if (readings.Length == 1)
                {
                    label.Text = FormatSensorReading(2, readings[0]) + $" {unit}";
                }
                else if (readings.Length == 3 || readings.Length == 4)
                {
                    label.Text = FormatSensorReading(2, readings);
                }
            }
            else
            {
                Console.WriteLine($"{label}: Label is null!");
            }
        }

        public static string FormatSensorReading(int decimalPlaces = 2, params double[] values)
        {
            string format = $"F{decimalPlaces}";
            var formattedValues = new List<string>();

            // Nomeia os eixos conforme o número de valores (X, Y, Z, W)
            var axisLabels = new[] { "X", "Y", "Z", "W" };
            for (int i = 0; i < values.Length && i < axisLabels.Length; i++)
            {
                formattedValues.Add($"{axisLabels[i]}: {values[i].ToString(format)}");
            }

            return string.Join(", ", formattedValues);
        }

        public static async Task ShowSensorNotSupportedMessage(string sensorName)
        {
            await Application.Current.MainPage.DisplayAlert("Sensor Not Supported", $"{sensorName} is not supported on this device.", "OK");
        }

    }

}

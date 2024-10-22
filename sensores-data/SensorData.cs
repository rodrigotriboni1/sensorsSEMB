using System.Text.Json.Serialization;

namespace sensores_data
{
    public class SensorData
    {
        public string SensorName { get; set; }

        public string SensorType { get; set; }

        public double ValueX { get; set; }

        public double ValueY { get; set; }

        public double ValueZ { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

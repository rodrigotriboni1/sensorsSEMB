using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace sensoresMAUISEMB
{
    public class SensorData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }  // Add the Id property!
        [JsonPropertyName("SensorName")]
        public string SensorName { get; set; }

        [JsonPropertyName("SensorType")]
        public string SensorType { get; set; }

        [JsonPropertyName("ValueX")]
        public double ValueX { get; set; }

        [JsonPropertyName("ValueY")]
        public double ValueY { get; set; }

        [JsonPropertyName("ValueZ")]
        public double ValueZ { get; set; }

        [JsonPropertyName("Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}

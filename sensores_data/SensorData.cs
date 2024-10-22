

namespace sensores_data
{
    public class SensorData
    {
        public int Id { get; set; }  // Add the Id property!
        public required string SensorName { get; set; }
        public required string SensorType { get; set; }
        public double ValueX { get; set; }
        public double ValueY { get; set; }
        public double ValueZ { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

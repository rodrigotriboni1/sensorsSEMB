namespace sensores_data.Models
{
    public class SensorStatus
    {
        public int Id { get; set; }
        public int SensorId { get; set; } // To link to the sensor
        public string Status { get; set; } // e.g., "Active", "Inactive", "Error"
        public DateTime Timestamp { get; set; }

    }
}

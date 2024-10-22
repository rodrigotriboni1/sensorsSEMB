using System.Threading.Tasks;

namespace sensores_data
{
    public interface ISensorService
    {
        Task<List<SensorData>> getAllSensorAsync();
        Task<SensorData> addSensor(SensorDTO sensorDTO);
    }

}


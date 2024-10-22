using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace sensores_data
{
    public class SensorService : ISensorService
    {
        private readonly SensorDataDbContext _sensorDataDbContext;
        private readonly IMapper _mapper;

        public SensorService(SensorDataDbContext studentPortalDbContext)
        {
            this._sensorDataDbContext = studentPortalDbContext;
        }
        public async Task<List<SensorData>> getAllSensorAsync()
        {
            var sensor = await _sensorDataDbContext.SensorData.ToListAsync();
            return sensor;
        }

        private SensorData MapSensorObject(SensorDTO sensorDTO)
        {
            SensorData sensor = new SensorData();
            sensor.SensorName = sensorDTO.SensorName;
            sensor.SensorType = sensorDTO.SensorType;
            return sensor;
        }
        public async Task<SensorData> addSensor(SensorDTO sensorDTO)
        {
            SensorData sensor = MapSensorObject(sensorDTO);
            _sensorDataDbContext.SensorData.Add(sensor);
            await _sensorDataDbContext.SaveChangesAsync();
            return sensor;
        }
        public SensorService(SensorDataDbContext sensorDataDbContext, IMapper mapper)
        {
            this._sensorDataDbContext = sensorDataDbContext;
            this._mapper = mapper;
        }

        public async Task<SensorData> addStudent(SensorDTO sensorDTO)
        {
            SensorData sensor = _mapper.Map<SensorData>(sensorDTO);
            _sensorDataDbContext.SensorData.Add(sensor);
            await _sensorDataDbContext.SaveChangesAsync();
            return sensor;
        }

    }
}

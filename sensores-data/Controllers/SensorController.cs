using Microsoft.AspNetCore.Mvc;

namespace sensores_data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _sensorService;
        public SensorController(ISensorService SensorService)
        {
            this._sensorService = SensorService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetSensor()
        {
            var sensor = await _sensorService.getAllSensorAsync();
            return Ok(sensor);

        }
        [HttpPost("save")]
        public async Task<IActionResult> AddSensor(SensorDTO sensorDTO)
        {
            var sensor = await _sensorService.addSensor(sensorDTO);
            return Ok(sensor);
        }
    }
}

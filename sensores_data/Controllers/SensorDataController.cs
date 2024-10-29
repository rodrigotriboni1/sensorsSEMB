using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace sensores_data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorDataController : ControllerBase
    {
        private readonly SensorDbContext _context;

        public SensorDataController(SensorDbContext context)
        {
            _context = context;
        }

        [HttpGet("updates")]
        public IActionResult GetSensorUpdates(DateTime? lastUpdate)
        {
            var updates = _context.SensorData
                .Where(s => lastUpdate == null || s.Timestamp > lastUpdate) // Get all if no lastUpdate 
                .OrderBy(s => s.Timestamp)
                .ToList();

            return Ok(updates);
        }

        [HttpPost]
        public async Task<IActionResult> PostSensorData([FromBody] SensorData sensorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            sensorData.Timestamp = DateTime.UtcNow; // Optionally set server-side timestamp
            _context.SensorData.Add(sensorData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSensorData", new { id = sensorData.Id }, sensorData); // Assuming you add an Id property
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorData>>> GetSensorData()
        {
            return await _context.SensorData.ToListAsync();
        }



    }
}


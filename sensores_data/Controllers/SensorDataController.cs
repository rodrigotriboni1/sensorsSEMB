using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sensores_data.Models;
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

        [HttpPost]
        public async Task<IActionResult> PostSensorData([FromBody] SensorData sensorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            sensorData.Timestamp = DateTime.UtcNow; //server-side timestamp
            _context.SensorData.Add(sensorData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSensorData", new { id = sensorData.Id }, sensorData);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorData>>> GetSensorData()
        {
            return await _context.SensorData.ToListAsync();
        }

        [HttpPost("status")] // New endpoint for SensorStatus
        public async Task<IActionResult> PostSensorStatus([FromBody] SensorStatus sensorStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            sensorStatus.Timestamp = DateTime.UtcNow;
            _context.SensorStatuses.Add(sensorStatus); // Use DbSet for SensorStatus
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSensorStatus", new { id = sensorStatus.Id }, sensorStatus); // New action name
        }

        [HttpGet("status")] // New endpoint to get all SensorStatus entries
        public async Task<ActionResult<IEnumerable<SensorStatus>>> GetSensorStatuses()
        {
            return await _context.SensorStatuses.ToListAsync();
        }


        [HttpGet("status/{id}")] // Get SensorStatus by ID
        public async Task<ActionResult<SensorStatus>> GetSensorStatus(int id)
        {
            var sensorStatus = await _context.SensorStatuses.FindAsync(id);

            if (sensorStatus == null)
            {
                return NotFound();
            }

            return sensorStatus;
        }

        //Example of updating Sensor Status
        [HttpPut("status/{id}")]
        public async Task<IActionResult> PutSensorStatus(int id, SensorStatus sensorStatus)
        {
            if (id != sensorStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(sensorStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorStatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        private bool SensorStatusExists(int id)
        {
            return (_context.SensorStatuses?.Any(e => e.Id == id)).GetValueOrDefault();
        }



}
}


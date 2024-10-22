using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace sensores_data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Hash password here
            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                return Unauthorized("Invalid login credentials");
            }

            // Return JWT token (optional)
            return Ok("Login successful");
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Verify hashed password
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }

}

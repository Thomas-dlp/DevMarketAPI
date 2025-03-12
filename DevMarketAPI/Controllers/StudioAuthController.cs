using DevMarketAPI.Data;
using DevMarketAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevMarketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudioAuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudioAuthController(AppDbContext appDbContext)
        {
            _context = appDbContext;
            Console.WriteLine("controller built");
        }

        // GET: api/<StudioCredentialsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudioCredentials>>> GetStudioCredentials()
        {
            return await _context.StudioCredentials.ToListAsync();
        }

        // GET api/<StudioCredentialsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StudioCredentialsController>
        [HttpPost("login")]
        public async Task<ActionResult<StudioCredentials>> LoginStudio(LoginStudioRequest loginStudioRequest)
        {
            var foundStudio = await _context.StudioCredentials.FirstOrDefaultAsync(context => context.Email.Equals(loginStudioRequest.Email));
            if (foundStudio == null || HashPassword(loginStudioRequest.Password) != foundStudio.PasswordHash) 
            {
                return Unauthorized(new {message="Invalid username or password"});
            }
            
            return Ok(new { message = "User logged in successfully", id = $"{foundStudio.Id}" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewStudio([FromBody]RegisterStudioRequest registerStudioRequest)
        {
            if (await _context.StudioCredentials.AnyAsync(studio => studio.Email == registerStudioRequest.Email))
                return BadRequest("Username already taken");

            var hashedPassword=HashPassword(registerStudioRequest.Password);
            var studioCredentials= new StudioCredentials(registerStudioRequest.Email,hashedPassword);
            _context.StudioCredentials.Add(studioCredentials);
            await _context.SaveChangesAsync();
            

            return Ok(new { message= "User registered successfully", id=$"{studioCredentials.Id}" });

        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}

using DevMarketAPI.Data;
using DevMarketAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevMarketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudioCredentialsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudioCredentialsController(AppDbContext appDbContext)
        {
            _context = appDbContext;
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
        [HttpPost]
        public async Task<ActionResult<StudioCredentials>> AddStudioCredentials(StudioCredentials studioCredentials)
        {
            _context.StudioCredentials.Add(studioCredentials);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = studioCredentials.Id }, studioCredentials);
        }

        

       
    }
}

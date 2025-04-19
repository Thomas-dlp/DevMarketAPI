using Devmarket.Infrastructure.Persistence;
using DevMarketAPI.DTOs;
using DevMarketAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevMarketAPI.Controllers
{
    [Route("api/devs")]
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DevController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        // GET: api/<DevController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dev>>> Get([FromQuery] Guid? studioId)
        {
            var query = _context.Devs.AsQueryable();

            if (studioId.HasValue)
            {
                query = query.Where(dev => dev.StudioId == studioId.Value);
            }

            var devs = await query.ToListAsync();
            return Ok(devs);
        }

        // POST api/<DevController>
        [HttpPost]
        public async Task<ActionResult<Dev>> Post([FromBody] DevDto devDto)
        {
            if (devDto == null)
            {
                return BadRequest("DevDto is null");
            }

            var foundStudio = await _context.StudioProfiles.FirstOrDefaultAsync(studio => studio.Id == devDto.StudioId);
            if(foundStudio is null) 
            {
                return BadRequest("No studio Found");
            }
            var dev = new Dev
            {
                Id = Guid.NewGuid(),
                Title = devDto.Name,
                Description = devDto.Description,
                ImageUrl = devDto.LogoUrl,
                StudioId = devDto.StudioId
            };
            _context.Devs.Add(dev);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dev.Id }, dev);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dev>> GetById(Guid id)
        {
            var dev = await _context.Devs.FindAsync(id);
            if (dev == null)
                return NotFound();

            return Ok(dev);
        }

       

        // PUT api/<DevController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DevController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

      
    }
}

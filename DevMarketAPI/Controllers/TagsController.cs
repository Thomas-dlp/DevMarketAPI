using Devmarket.Infrastructure.Persistence;
using DevMarketAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevMarketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        AppDbContext _context;

        public TagsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<TagsController>
        [HttpGet]
        [HttpGet("light")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()  //todo as tag not different than lightelement, should we send them diretly from route tags.
        {
            var tags = await _context.Tags.ToListAsync();
            return Ok(tags);
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> Post([FromBody] string tagDto)
        {
            if (tagDto == null)
            {
                return BadRequest("TagDto is null");
            }

            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Title = tagDto,
            };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTags), new { id = tag.Id }, tag);
        }
    }
}

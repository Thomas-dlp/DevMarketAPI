using DevMarketAPI.Data;
using DevMarketAPI.DTOs;
using DevMarketAPI.Models;
using Microsoft.AspNetCore.Http;
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
    }
}

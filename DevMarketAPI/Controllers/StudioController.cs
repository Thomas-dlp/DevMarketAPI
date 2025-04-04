using AutoMapper;
using DevMarketAPI.Data;
using DevMarketAPI.DTOs;
using DevMarketAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevMarketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public StudioController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}/page")]
        public async Task<ActionResult<StudioProfile>> GetStudioPage(Guid id)
        {
            var studioProfile = await _context.StudioProfiles.FindAsync(id);
            if (studioProfile == null)
            {
                return BadRequest();

                
            }

            return studioProfile; //todo: join elements from other tables;
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<StudioProfile>> GetStudioProfile(Guid id)
        {
            var studioProfile = await _context.StudioProfiles.FindAsync(id);
            if (studioProfile == null)
            {
                return await CreateNewStudioProfile(id);
            }
            return studioProfile;
        }

        [HttpPost("{id}/profile")]
        public async Task<ActionResult<StudioProfile>> CreateNewStudioProfile(Guid id)
        {
            var studioProfile = await _context.StudioProfiles.FindAsync(id);
            if (studioProfile != null)
            {
                return BadRequest(new { mesage = "Studio profile already exists" });
            }
            //todo: check id exisgt in credzentials table

            StudioProfile newStudioProfile = new() { Id = id };
            _context.StudioProfiles.Add(newStudioProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudioProfile), new { id = newStudioProfile.Id }, newStudioProfile);
        }

        [HttpPatch("{id}/profile")]
        public async Task<IActionResult> UpdateUserProfile(Guid id, UpdateStudioProfileDto updateStudioProfileDto)
        {
            var studioProfile = await _context.StudioProfiles.FindAsync(id);
            if (studioProfile == null)
            {
                return BadRequest();
            }

            _mapper.Map(updateStudioProfileDto, studioProfile);
            _context.Entry(studioProfile).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { studioProfile });

        }


        [HttpPost("batchFetch")]
        public async Task<ActionResult<List<IDisplayableElement>>> GetBatchElements([FromBody] List<IDisplayableElementReference> references)
        {
            if (references == null || references.Count == 0)
            {
                return BadRequest("No references provided.");
            }

            // Fetch all elements in parallel
            var tasks = references.Select(refObj => FetchElement(refObj));
            var results = await Task.WhenAll(tasks);

            return Ok(results.Where(e => e != null)); // Filter out null values (if any)
        }

        private async Task<IDisplayableElement?> FetchElement(IDisplayableElementReference refObj)
        {
            switch (refObj.Type)
            {
                case DisplayableElementType.Dev:
                    return await _context.Devs.FindAsync(refObj.Id);
                case DisplayableElementType.Post:
                    return await _context.Posts.FindAsync(refObj.Id);
                case DisplayableElementType.TradingStatus:
                    return await _context.TradingStatuses.FindAsync(refObj.Id);
                default:
                    return null; // Return null if type is unknown
            }
        }


    }
}

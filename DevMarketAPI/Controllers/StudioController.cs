using AutoMapper;
using Devmarket.Infrastructure.Persistence;
using DevMarketAPI.DTOs;
using DevMarketAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevMarketAPI.Controllers
{
    [Authorize(Policy = "StudioAccessPolicy")]
    [Route("api/studios")]
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

        [AllowAnonymous]
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

        [HttpPut("{studioId}/actualities")]
        public async Task<ActionResult<IDisplayableElement>> AddActuality(Guid studioId, DisplayableElementReference referenceDto)
        {
            // Check if the reference already exists
            var existingReference = _context.ReferenceLinks
                .FirstOrDefault(r => r.StudioId == studioId && r.DisplayableElementId == referenceDto.DisplayableElementId && r.DisplayableElementType == referenceDto.DisplayableElementType);
            if (existingReference != null)
            {
                return BadRequest("This element is already added to the studio's activities.");
            }

            // Add the new reference
            var referenceLink = new DisplayableElementReferenceLink
            {
                Id = new Guid(),
                DisplayableElementId = referenceDto.DisplayableElementId,
                DisplayableElementType = referenceDto.DisplayableElementType,
                Order = referenceDto.Order,
                StudioId = studioId
            };
            _context.ReferenceLinks.Add(referenceLink);

            await _context.SaveChangesAsync();

            IDisplayableElement addedActuality;
            switch (referenceLink.DisplayableElementType)
            {
                case DisplayableElementType.Dev:
                    addedActuality = await _context.Devs.FindAsync(referenceLink.DisplayableElementId);
                    break;
                case DisplayableElementType.Post:
                    addedActuality = await _context.Posts.FindAsync(referenceLink.DisplayableElementId);
                    break;
                case DisplayableElementType.TradingStatus:
                    addedActuality = await _context.TradingStatuses.FindAsync(referenceLink.DisplayableElementId);
                    break;
                default:
                    return BadRequest("Invalid reference type.");
            }

            return Ok(addedActuality); // Return the newly added element
        }

        [HttpDelete("{studioId}/actualities/{actualityId}")]
        public async Task<ActionResult<IDisplayableElement>> RemoveActuality(Guid studioId, Guid actualityId)
        {
            // Check if the reference already exists
            var existingReference = _context.ReferenceLinks
                .FirstOrDefault(r => r.StudioId == studioId && r.DisplayableElementId == actualityId);
            if (existingReference == null)
            {
                return BadRequest("No element to remove");
            }

            _context.ReferenceLinks.Remove(existingReference);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{studioId}/actualities")]
        public async Task<ActionResult<List<IDisplayableElement>>> GetAllActualities(Guid studioId) //todo fix types so that we can simplyfy this
        {
            var foundReferences = await _context.ReferenceLinks
                .Where(reference => reference.StudioId == studioId)
                .ToListAsync(); ;
                

            var devIds = foundReferences
                .Where(reference => reference.DisplayableElementType == DisplayableElementType.Dev)
                .Select(reference => reference.DisplayableElementId)
                .ToList();

            var postIds = foundReferences
                .Where(reference => reference.DisplayableElementType == DisplayableElementType.Post)
                .Select(reference => reference.DisplayableElementId)
                .ToList();

            var tradingStatusIds = foundReferences
                .Where(reference => reference.DisplayableElementType == DisplayableElementType.TradingStatus)
                .Select(reference => reference.DisplayableElementId)
                .ToList();

            // Fetch all relevant Devs in a single query
            var devs = await _context.Devs
                .Where(dev => devIds.Contains(dev.Id))
                .ToListAsync();

            // Fetch all relevant Posts in a single query
            var posts = await _context.Posts
                .Where(post => postIds.Contains(post.Id))
                .ToListAsync();

            // Fetch all relevant TradingStatuses in a single query
            var tradingStatuses = await _context.TradingStatuses
                .Where(status => tradingStatusIds.Contains(status.Id))
                .ToListAsync();

            // Now map each reference to its corresponding element
            List<IDisplayableElement> actualities = new List<IDisplayableElement>();

            actualities.AddRange(devs);
            actualities.AddRange(posts);
            actualities.AddRange(tradingStatuses);

            return Ok(actualities);
        }


        [HttpPost("{studioId}/activities/batchFetch")]
        public async Task<ActionResult<List<IDisplayableElement>>> GetBatchElements([FromBody] List<DisplayableElementReference> references)
        {
            if (references == null || references.Count == 0)
            {
                return BadRequest("No references provided.");
            }

            // Fetch all elements in parallel
            var tasks = references.Select(refObj => FetchElement(refObj));
            var results = await Task.WhenAll(tasks);

            return Ok(results.Where(e => e != null));
        }

        private async Task<IDisplayableElement?> FetchElement(DisplayableElementReference refObj)
        {
            switch (refObj.DisplayableElementType)
            {
                case DisplayableElementType.Dev:
                    return await _context.Devs.FindAsync(refObj.DisplayableElementId);
                case DisplayableElementType.Post:
                    return await _context.Posts.FindAsync(refObj.DisplayableElementId);
                case DisplayableElementType.TradingStatus:
                    return await _context.TradingStatuses.FindAsync(refObj.DisplayableElementId);
                default:
                    return null; // Return null if type is unknown
            }
        }

        [AllowAnonymous]
        [HttpGet("light")]
        public async Task<ActionResult<IEnumerable<LightElementDto>>> GetLightStudios()
        {
            var lightStudios = await _context.StudioProfiles.Select(studio => new LightElementDto
            {
                Id = studio.Id,
                Title = studio.Name,
            }).ToListAsync();

            return Ok(lightStudios);

        }


        [HttpGet("{id}/devs/light")]
        public async Task<ActionResult<IEnumerable<LightElementDto>>> GetLightDevs(Guid id)
        {
            var lightDevs = await _context.Devs.Where(dev => dev.StudioId == id).Select(dev=> new LightElementDto
            {
                Id = dev.Id,
                Title = dev.Title,
            }).ToListAsync();
           
            return Ok(lightDevs);

        }

        [HttpGet("{id}/posts/light")]
        public async Task<ActionResult<IEnumerable<LightElementDto>>> GetLightPosts(Guid id) {
            var lightPosts = await _context.Posts.Where(posts => posts.StudioId == id).Select(post => new LightElementDto
            {
                Id = post.Id,
                Title = post.Title,
            }).ToListAsync();

            return Ok(lightPosts);
        }

        [HttpGet("{id}/tradingStatuses/light")]
        public async Task<ActionResult<IEnumerable<LightElementDto>>> GetLightTradingStatuses(Guid id)
        {
            var lightTradingStatuses = await _context.TradingStatuses.Where(statuses => statuses.StudioId == id).Select(status => new LightElementDto
            {
                Id = status.Id,
                Title = status.Title,
            }).ToListAsync();

            return Ok(lightTradingStatuses);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineApi.Context;
using WineApi.Model;
using WineApi.Model.DTO;

namespace WineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FermentationEntriesController : ControllerBase
    {
        private readonly WineDbContext _context;

        public FermentationEntriesController(WineDbContext context)
        {
            _context = context;
        }


        // GET: api/FermentationEntries/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FermentationEntryDTO>> GetFermentationEntry(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var fermentationEntry = await _context.FermentationEntries.FindAsync(id);

            if (fermentationEntry == null)
            {
                return NotFound();
            }

            var wine = fermentationEntry.Wine;

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }

            return FermentationEntryDTO.MapFermentationEntryToDto(fermentationEntry);
        }

        // PUT: api/FermentationEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFermentationEntry(int id, FermentationEntryDTO fermentationEntry)
        {
            if (id != fermentationEntry.Id)
            {
                return BadRequest();
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var wine = _context.Wines.Find(fermentationEntry.WineId);

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound(); 
            }


            _context.Entry(FermentationEntryDTO.MapDtoToFermentationEntry(fermentationEntry)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FermentationEntryExists(id))
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

        // POST: api/FermentationEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FermentationEntryDTO>> PostFermentationEntry(FermentationEntryDTO fermentationEntry)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized(); 
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }


            if (fermentationEntry == null)
            {
                return NotFound();
            }


            var wine = _context.Wines.Find(fermentationEntry.WineId);

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }

            var newFermentationEntry = FermentationEntryDTO.MapDtoToFermentationEntry(fermentationEntry);
            _context.FermentationEntries.Add(newFermentationEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFermentationEntry", new { id = newFermentationEntry.Id }, newFermentationEntry);
        }

        // DELETE: api/FermentationEntries/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFermentationEntry(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }


            var fermentationEntry = await _context.FermentationEntries.FindAsync(id);
            if (fermentationEntry == null)
            {
                return NotFound();
            }

            var wine = _context.Wines.Find(fermentationEntry.WineId);

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }

            _context.FermentationEntries.Remove(fermentationEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FermentationEntryExists(int id)
        {
            return _context.FermentationEntries.Any(e => e.Id == id);
        }
    }
}

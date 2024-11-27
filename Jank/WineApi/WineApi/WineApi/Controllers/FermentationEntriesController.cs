using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: api/FermentationEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FermentationEntryDTO>>> GetFermentationEntries()
        {
            return await _context.FermentationEntries.Select(f => FermentationEntryDTO.MapFermentationEntryToDto(f)).ToListAsync();
        }

        // GET: api/FermentationEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FermentationEntryDTO>> GetFermentationEntry(int id)
        {
            var fermentationEntry = await _context.FermentationEntries.FindAsync(id);

            if (fermentationEntry == null)
            {
                return NotFound();
            }

            return FermentationEntryDTO.MapFermentationEntryToDto(fermentationEntry);
        }

        // PUT: api/FermentationEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFermentationEntry(int id, FermentationEntryDTO fermentationEntry)
        {
            if (id != fermentationEntry.Id)
            {
                return BadRequest();
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
        public async Task<ActionResult<FermentationEntryDTO>> PostFermentationEntry(FermentationEntryDTO fermentationEntry)
        {
            _context.FermentationEntries.Add(FermentationEntryDTO.MapDtoToFermentationEntry(fermentationEntry));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFermentationEntry", new { id = fermentationEntry.Id }, fermentationEntry);
        }

        // DELETE: api/FermentationEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFermentationEntry(int id)
        {
            var fermentationEntry = await _context.FermentationEntries.FindAsync(id);
            if (fermentationEntry == null)
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

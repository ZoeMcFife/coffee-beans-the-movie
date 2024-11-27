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
    public class WinesController : ControllerBase
    {
        private readonly WineDbContext _context;

        public WinesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/Wines
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<WineDTO>>> GetWines(int userId)
        {
            return await _context.Wines
                .Select(w => WineDTO.MapWineToDto(w))
                .Where(w => w.UserId.Equals(userId))
                .ToListAsync();
        }

        // GET: api/Wines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WineDTO>> GetWine(int id)
        {
            var wine = await _context.Wines.FindAsync(id);

            if (wine == null)
            {
                return NotFound();
            }

            return WineDTO.MapWineToDto(wine);
        }

        // PUT: api/Wines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWine(int id, WineDTO wine)
        {
            if (id != wine.Id)
            {
                return BadRequest();
            }

            _context.Entry(WineDTO.MapDtoToWine(wine)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WineExists(id))
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

        // POST: api/Wines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WineDTO>> PostWine(WineDTO wine)
        {
            _context.Wines.Add(WineDTO.MapDtoToWine(wine));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWine", new { id = wine.Id }, wine);
        }

        // DELETE: api/Wines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWine(int id)
        {
            var wine = await _context.Wines.FindAsync(id);
            if (wine == null)
            {
                return NotFound();
            }

            _context.Wines.Remove(wine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/Additives")]
        public async Task<ActionResult<IEnumerable<WineDTO>>> GetAdditives(int id)
        {
            var wine = await _context.Wines
               .Include(w => w.Additives)
               .FirstOrDefaultAsync(w => w.Id == id);

            if (wine == null)
            {
                return NotFound(); // Return 404 if wine is not found
            }

            // Map the additives to AdditiveDTO
            var additivesDto = wine.Additives.Select(AdditiveDTO.MapAdditiveToDto);

            return Ok(additivesDto);
        }

        [HttpGet("{id}/FermentationEntries")]
        public async Task<ActionResult<IEnumerable<WineDTO>>> GetFermentationEntries(int id)
        {
            var wine = await _context.Wines
               .Include(w => w.FermentationEntries)
               .FirstOrDefaultAsync(w => w.Id == id);

            if (wine == null)
            {
                return NotFound(); // Return 404 if wine is not found
            }

            // Map the additives to AdditiveDTO
            var additivesDto = wine.FermentationEntries.Select(FermentationEntryDTO.MapFermentationEntryToDto);

            return Ok(additivesDto);
        }

        private bool WineExists(int id)
        {
            return _context.Wines.Any(e => e.Id == id);
        }
    }
}

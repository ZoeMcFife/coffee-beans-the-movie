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
    public class AdditivesController : ControllerBase
    {
        private readonly WineDbContext _context;

        public AdditivesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/Additives
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdditiveDTO>>> GetAdditives()
        {
            return await _context.Additives.Select(a => AdditiveDTO.MapAdditiveToDto(a)).ToListAsync();
        }

        // GET: api/Additives/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdditiveDTO>> GetAdditive(int id)
        {
            var additive = await _context.Additives.FindAsync(id);

            if (additive == null)
            {
                return NotFound();
            }

            return AdditiveDTO.MapAdditiveToDto(additive);
        }

        // PUT: api/Additives/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdditive(int id, AdditiveDTO additive)
        {
            if (id != additive.Id)
            {
                return BadRequest();
            }



            _context.Entry(AdditiveDTO.MapDtoToAdditive(additive)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdditiveExists(id))
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

        // POST: api/Additives
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdditiveDTO>> PostAdditive(AdditiveDTO additive)
        {
            _context.Additives.Add(AdditiveDTO.MapDtoToAdditive(additive));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdditive", new { id = additive.Id }, additive);
        }

        // DELETE: api/Additives/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdditive(int id)
        {
            var additive = await _context.Additives.FindAsync(id);
            if (additive == null)
            {
                return NotFound();
            }

            _context.Additives.Remove(additive);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdditiveExists(int id)
        {
            return _context.Additives.Any(e => e.Id == id);
        }
    }
}

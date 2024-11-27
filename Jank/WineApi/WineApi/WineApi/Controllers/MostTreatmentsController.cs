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
    public class MostTreatmentsController : ControllerBase
    {
        private readonly WineDbContext _context;

        public MostTreatmentsController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/MostTreatments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MostTreatmentDTO>>> GetMostTreatments()
        {
            return await _context.MostTreatments.Select(m => MostTreatmentDTO.MapMostTreatmentToDto(m)).ToListAsync();
        }

        // GET: api/MostTreatments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MostTreatmentDTO>> GetMostTreatment(int id)
        {
            var mostTreatment = await _context.MostTreatments.FindAsync(id);

            if (mostTreatment == null)
            {
                return NotFound();
            }

            return MostTreatmentDTO.MapMostTreatmentToDto(mostTreatment);
        }

        // PUT: api/MostTreatments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMostTreatment(int id, MostTreatmentDTO mostTreatment)
        {
            if (id != mostTreatment.Id)
            {
                return BadRequest();
            }

            _context.Entry(MostTreatmentDTO.MapDtoToMostTreatment(mostTreatment)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MostTreatmentExists(id))
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

        // POST: api/MostTreatments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MostTreatmentDTO>> PostMostTreatment(MostTreatmentDTO mostTreatment)
        {
            _context.MostTreatments.Add(MostTreatmentDTO.MapDtoToMostTreatment(mostTreatment));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMostTreatment", new { id = mostTreatment.Id }, mostTreatment);
        }

        // DELETE: api/MostTreatments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMostTreatment(int id)
        {
            var mostTreatment = await _context.MostTreatments.FindAsync(id);
            if (mostTreatment == null)
            {
                return NotFound();
            }

            _context.MostTreatments.Remove(mostTreatment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MostTreatmentExists(int id)
        {
            return _context.MostTreatments.Any(e => e.Id == id);
        }
    }
}

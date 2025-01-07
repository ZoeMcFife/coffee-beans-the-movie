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
    public class AdditivesController : ControllerBase
    {
        private readonly WineDbContext _context;

        public AdditivesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/Additives/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AdditiveDTO>> GetAdditive(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized(); // Return 401 if no userId claim is found
            }

            // Parse the userId from the claim
            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var additive = await _context.Additives.Include(a => a.Wine).FirstOrDefaultAsync(a => a.Id == id);

            if (additive == null)
            {
                return NotFound();
            }

            var wine = additive.Wine;

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound(); // Return 404 if wine is not found
            }


            return AdditiveDTO.MapAdditiveToDto(additive);
        }

        // PUT: api/Additives/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAdditive(int id, AdditiveDTO additive)
        {
            if (id != additive.Id)
            {
                return BadRequest();
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized(); // Return 401 if no userId claim is found
            }

            // Parse the userId from the claim
            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var wine = _context.Wines.Find(additive.WineId);

            if (wine == null)
            {
                return NotFound();
            }

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
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
        [Authorize]
        public async Task<ActionResult<AdditiveDTO>> PostAdditive(AdditiveDTO additive)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized(); // Return 401 if no userId claim is found
            }

            // Parse the userId from the claim
            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var wine = _context.Wines.Find(additive.WineId);

            if (wine == null)
            {
                return NotFound();
            }

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
            }

            var newAdditive = AdditiveDTO.MapDtoToAdditive(additive);
            _context.Additives.Add(newAdditive);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdditive", new { id = newAdditive.Id }, newAdditive);
        }

        // DELETE: api/Additives/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdditive(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized(); // Return 401 if no userId claim is found
            }

            // Parse the userId from the claim
            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var additive = await _context.Additives.FindAsync(id);
            if (additive == null)
            {
                return NotFound();
            }

            var wine = _context.Wines.Find(additive.WineId);

            if (wine == null)
            {
                return NotFound();
            }

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
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

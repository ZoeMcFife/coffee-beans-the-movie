using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class WinesController : ControllerBase
    {
        private readonly WineDbContext _context;

        public WinesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/Wines/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<WineDTO>> GetWine(int id)
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

            var wine = await _context.Wines.FindAsync(id);

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

            return WineDTO.MapWineToDto(wine);
        }

        // PUT: api/Wines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutWine(int id, WineDTO wine)
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

            if (wine != null)
            {
                if (wine.UserId != userId)
                {
                    return Unauthorized();
                }
            }

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
        [Authorize]
        public async Task<ActionResult<WineDTO>> PostWine(WineDTO wine)
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

            if (wine.UserId != userId)
            {
                return Unauthorized();
            }

            _context.Wines.Add(WineDTO.MapDtoToWine(wine));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWine", new { id = wine.Id }, wine);
        }

        // DELETE: api/Wines/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWine(int id)
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

            var wine = await _context.Wines.FindAsync(id);

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

            _context.Wines.Remove(wine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/Additives")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WineDTO>>> GetAdditives(int id)
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

            var wine = await _context.Wines
               .Include(w => w.Additives)
               .FirstOrDefaultAsync(w => w.Id == id);

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

            // Map the additives to AdditiveDTO
            var additivesDto = wine.Additives.Select(AdditiveDTO.MapAdditiveToDto);

            return Ok(additivesDto);
        }

        [HttpGet("{id}/FermentationEntries")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WineDTO>>> GetFermentationEntries(int id)
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

            var wine = await _context.Wines
               .Include(w => w.FermentationEntries)
               .FirstOrDefaultAsync(w => w.Id == id);

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

            // Map the additives to AdditiveDTO
            var additivesDto = wine.FermentationEntries.Select(FermentationEntryDTO.MapFermentationEntryToDto);

            return Ok(additivesDto);
        }

        private bool WineExists(int id)
        {
            return _context.Wines.Any(e => e.Id == id);
        }

        /* 
         *   Most Treatments
         */


        // GET: api/MostTreatments/5
        [HttpGet("{id}/MostTreatment")]
        [Authorize]
        public async Task<ActionResult<MostTreatmentDTO>> GetMostTreatment(int id)
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

            var wine = await _context.Wines
               .Include(w => w.MostTreatment)
               .FirstOrDefaultAsync(w => w.Id == id);

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

            var mostTreatment = wine.MostTreatment;

            if (mostTreatment == null)
            {
                return NotFound();
            }

            return MostTreatmentDTO.MapMostTreatmentToDto(mostTreatment);
        }

        // PUT: api/MostTreatments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/MostTreatment")]
        [Authorize]
        public async Task<IActionResult> PutMostTreatment(int id, MostTreatmentDTO mostTreatment)
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

            var wine = await _context.Wines
               .Include(w => w.MostTreatment)
               .FirstOrDefaultAsync(w => w.Id == id);

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

            if (wine.MostTreatment == null)
            {
                return NotFound();
            }

            if (wine.MostTreatment.Id != mostTreatment.Id)
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
        [HttpPost("{id}/MostTreatment")]
        [Authorize]
        public async Task<ActionResult<MostTreatmentDTO>> PostMostTreatment(int id, MostTreatmentDTO mostTreatment)
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

            var wine = await _context.Wines
               .Include(w => w.MostTreatment)
               .FirstOrDefaultAsync(w => w.Id == id);

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
            

            _context.MostTreatments.Add(MostTreatmentDTO.MapDtoToMostTreatment(mostTreatment));
            await _context.SaveChangesAsync();

            wine.MostTreatment = _context.MostTreatments.FirstOrDefault(w => w.Id == mostTreatment.Id);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMostTreatment", new { id = mostTreatment.Id }, mostTreatment);
        }

        // DELETE: api/MostTreatments/5
        [HttpDelete("{id}/MostTreatment")]
        [Authorize]
        public async Task<IActionResult> DeleteMostTreatment(int id)
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

            var wine = await _context.Wines
               .Include(w => w.MostTreatment)
               .FirstOrDefaultAsync(w => w.Id == id);

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

            if (wine.MostTreatment  == null)
            {
                return NotFound();
            }

            var mostTreatment = await _context.MostTreatments.FindAsync(wine.MostTreatment.Id);
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

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
using WineApi.Helpers;
using WineApi.Model;
using WineApi.Model.DTO;

namespace WineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WinesController : ControllerBase
    {
        private readonly WineDbContext _context;

        private AuthHelper authHelper;

        public WinesController(WineDbContext context)
        {
            _context = context;
        }


        // GET: api/Users/Wines
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Wine>>> GetWines()
        {
            var results = authHelper.GetAuthenticatedUser(this);
                
            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wines = await _context.Wines
                .Where(w => w.UserId == results.UserId)
                .ToListAsync(); 

            return Ok(wines);
        }

        // GET: api/Wines/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Wine>> GetWine(int id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.Wines.FindAsync(id);

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }

            return wine;
        }

        // PUT: api/Wines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutWine(Guid id, Wine wine)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (id != wine.Id)
            {
                return BadRequest();
            }

            _context.Entry(wine).State = EntityState.Modified;

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
        public async Task<ActionResult<Wine>> PostWine(Wine wine)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            wine.UserId = (Guid)results.UserId;

            if (wine.UserId != results.UserId)
            {
                return Unauthorized();
            }

            _context.Wines.Add(wine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWine", new { id = wine.Id }, wine);
        }

        // DELETE: api/Wines/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWine(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.Wines.FindAsync(id);

            if (wine != null)
            {
                if (wine.UserId != results.UserId) 
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
        public async Task<ActionResult<IEnumerable<Additive>>> GetAdditives(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.Wines
               .Include(w => w.Additives)
               .FirstOrDefaultAsync(w => w.Id == id);

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }

            var additivesDto = wine.Additives;

            return Ok(additivesDto);
        }

        [HttpGet("{id}/FermentationEntries")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FermentationEntry>>> GetFermentationEntries(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.Wines
               .Include(w => w.FermentationEntries)
               .FirstOrDefaultAsync(w => w.Id == id);

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }

            var fermentationEntries = wine.FermentationEntries;

            return Ok(fermentationEntries);
        }

        private bool WineExists(Guid id)
        {
            return _context.Wines.Any(e => e.Id == id);
        }

        /* 
         *   Most Treatments
         */


        // GET: api/MostTreatments/5
        [HttpGet("{id}/MostTreatment")]
        [Authorize]
        public async Task<ActionResult<MostTreatment>> GetMostTreatment(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.Wines
               .Include(w => w.MostTreatment)
               .FirstOrDefaultAsync(w => w.Id == id);

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }

            var mostTreatment = wine.MostTreatment;

            if (mostTreatment == null)
            {
                return NotFound();
            }

            return Ok(mostTreatment);
        }

        // PUT: api/MostTreatments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/MostTreatment")]
        [Authorize]
        public async Task<IActionResult> PutMostTreatment(Guid id, MostTreatment mostTreatment)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.Wines
               .Include(w => w.MostTreatment)
               .FirstOrDefaultAsync(w => w.Id == id);

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }

            if (wine.MostTreatment == null)
            {
                return NotFound();
            }

            if (wine.MostTreatment.Id != mostTreatment.Id)
            {
                return BadRequest();
            }

            _context.Entry(mostTreatment).State = EntityState.Modified;

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
        public async Task<ActionResult<MostTreatment>> PostMostTreatment(Guid id, MostTreatment mostTreatment)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.Wines
               .Include(w => w.MostTreatment)
               .FirstOrDefaultAsync(w => w.Id == id);

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
            }
            

            _context.MostTreatments.Add(mostTreatment);
            await _context.SaveChangesAsync();

            wine.MostTreatment = _context.MostTreatments.FirstOrDefault(w => w.Id == mostTreatment.Id);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMostTreatment", new { id = mostTreatment.Id }, mostTreatment);
        }

        // DELETE: api/MostTreatments/5
        [HttpDelete("{id}/MostTreatment")]
        [Authorize]
        public async Task<IActionResult> DeleteMostTreatment(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.Wines
               .Include(w => w.MostTreatment)
               .FirstOrDefaultAsync(w => w.Id == id);

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wine == null)
            {
                return NotFound();
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

        private bool MostTreatmentExists(Guid id)
        {
            return _context.MostTreatments.Any(e => e.Id == id);
        }
    }
}

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
using WineApi.Model.Contraints;
using WineApi.Model.DTO;

namespace WineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WineBarrelsController : ControllerBase
    {
        private readonly WineDbContext _context;

        private AuthHelper authHelper = new AuthHelper();

        public WineBarrelsController(WineDbContext context)
        {
            _context = context;
        }


        // GET: api/Users/Wines
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WineBarrel>>> GetWineBarrels()
        {
            var results = authHelper.GetAuthenticatedUser(this);
                
            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wines = await _context.WineBarrels
                .Where(w => w.UserId == results.UserId)
                .ToListAsync(); 

            return Ok(wines);
        }

        // GET: api/Wines/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<WineBarrel>> GetWineBarrel(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.WineBarrels.FindAsync(id);

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

        [HttpGet("{id}/WineBarrelHistory")]
        [Authorize]
        public async Task<ActionResult<List<WineBarrelHistory>>> GetWineBarrelHistory(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var barrel = await _context.WineBarrels.FindAsync(id);

            if (barrel == null)
            {
                return NotFound();
            }

            if (barrel.UserId != results.UserId)
            {
                return Unauthorized();
            }

            var wineBarrelHistory = await _context.WineBarrelHistories.Where(h => h.WineBarrelId == barrel.Id).ToListAsync();

            return Ok(wineBarrelHistory);
        }

        [HttpPost("{id}/InsertWine/{wineTypeId}/{startDate}")]
        [Authorize]
        public async Task<IActionResult> InsertNewWine(Guid id, Guid wineTypeId, DateTime startDate)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var barrel = await _context.WineBarrels.FindAsync(id);

            if (barrel == null)
            {
                return NotFound();
            }

            if (barrel.UserId != results.UserId)
            {
                return Unauthorized();
            }

            if (barrel.CurrentWineBarrelHistoryId != null)
            {
                return BadRequest("Remove current Wine before adding new Wine");
            }

            var wineType = await _context.WineTypes.FindAsync(wineTypeId);

            if (wineType == null)
            {
                return NotFound();
            }

            var newBarrelHistory = new WineBarrelHistory
            {
                WineBarrelId = id,
                WineTypeId = wineTypeId,
                StartDate = startDate
            };

            await _context.WineBarrelHistories.AddAsync(newBarrelHistory);

            barrel.CurrentWineBarrelHistoryId = newBarrelHistory.Id;

            _context.Entry(barrel).State = EntityState.Modified;


            await _context.SaveChangesAsync();

            return Ok(newBarrelHistory);
        }

        [HttpPost("{id}/RemoveCurrentWine/{endDate}")]
        [Authorize]
        public async Task<IActionResult> RemoveCurrentWine(Guid id, DateTime endDate)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var barrel = await _context.WineBarrels.FindAsync(id);

            if (barrel == null)
            {
                return NotFound();
            }

            if (barrel.UserId != results.UserId)
            {
                return Unauthorized();
            }

            if (barrel.CurrentWineBarrelHistoryId == null)
            {
                return BadRequest("No Wine to remove");
            }

            var currentHistory = await _context.WineBarrelHistories.FindAsync(barrel.CurrentWineBarrelHistoryId);

            currentHistory.EndDate = endDate;

            _context.Entry(currentHistory).State = EntityState.Modified;

            barrel.CurrentWineBarrelHistoryId = null;

            _context.Entry(barrel).State = EntityState.Modified;

            _context.SaveChanges();

            return Ok(currentHistory);
        }

        [HttpGet("{id}/WineType")]
        [Authorize]
        public async Task<ActionResult<WineType>> GetCurrentWineType(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var barrel = await _context.WineBarrels.FindAsync(id);

            if (barrel == null)
            {
                return NotFound();
            }

            if (barrel.UserId != results.UserId)
            {
                return Unauthorized();
            }

            if (barrel.CurrentWineTypeId == null)
            {
                return NotFound();
            }

            var type = await _context.WineTypes.FindAsync(barrel.CurrentWineTypeId);

            return Ok(type);
        }

        [HttpGet("{id}/CurrentWineHistory")]
        [Authorize]
        public async Task<ActionResult<WineBarrelHistory>> GetCurrentWineBarrelHistory(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var barrel = await _context.WineBarrels.FindAsync(id);

            if (barrel == null)
            {
                return NotFound();
            }

            if (barrel.UserId != results.UserId)
            {
                return Unauthorized();
            }

            if (barrel.CurrentWineBarrelHistoryId == null)
            {
                return NotFound();
            }

            var wineBarrelHistory = await _context.WineBarrelHistories.FindAsync(barrel.CurrentWineBarrelHistoryId);

            return Ok(wineBarrelHistory);
        }



        // PUT: api/Wines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutWineBarrel(Guid id, WineBarrel wine)
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

            var (isValid, Error) = WineBarrelConstraints.CheckBarrel(wine);

            if (!isValid)
            {
                return BadRequest(Error);
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
                if (!WineBarrelExists(id))
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
        public async Task<ActionResult<WineBarrel>> PostWineBarrel(WineBarrel wine)
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

            var (isValid, Error) = WineBarrelConstraints.CheckBarrel(wine);

            if (!isValid)
            {
                return BadRequest(Error);
            }


            _context.WineBarrels.Add(wine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWine", new { id = wine.Id }, wine);
        }

        // DELETE: api/Wines/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWineBarrel(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = await _context.WineBarrels.FindAsync(id);

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

            _context.WineBarrels.Remove(wine);
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

            var wine = await _context.WineBarrels
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

            var wine = await _context.WineBarrels
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

        private bool WineBarrelExists(Guid id)
        {
            return _context.WineBarrels.Any(e => e.Id == id);
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

            var wine = await _context.WineBarrels
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

            var wine = await _context.WineBarrels
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

            var wine = await _context.WineBarrels
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

            var wine = await _context.WineBarrels
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

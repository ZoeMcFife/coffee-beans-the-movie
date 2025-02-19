using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FermentationEntriesController : ControllerBase
    {
        private readonly WineDbContext _context;
        private AuthHelper authHelper;

        public FermentationEntriesController(WineDbContext context)
        {
            _context = context;
        }


        // GET: api/FermentationEntries/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FermentationEntry>> GetFermentationEntry(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }


            var fermentationEntry = await _context.FermentationEntries.FindAsync(id);

            if (fermentationEntry == null)
            {
                return NotFound();
            }

            var wineId = fermentationEntry.WineId;

            if (wineId != null)
            {
                var wine = await _context.Wines.FindAsync(wineId);

                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wineId == null)
            {
                return NotFound();
            }

            return fermentationEntry;
        }

        // PUT: api/FermentationEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFermentationEntry(Guid id, FermentationEntry fermentationEntry)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            if (id != fermentationEntry.Id)
            {
                return BadRequest();
            }

            var wine = _context.Wines.Find(fermentationEntry.WineId);

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


            _context.Entry(fermentationEntry).State = EntityState.Modified;

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
        public async Task<ActionResult<FermentationEntry>> PostFermentationEntry(FermentationEntry fermentationEntry)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }


            if (fermentationEntry == null)
            {
                return NotFound();
            }


            var wine = _context.Wines.Find(fermentationEntry.WineId);

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

            var newFermentationEntry = fermentationEntry;
            _context.FermentationEntries.Add(newFermentationEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFermentationEntry", new { id = newFermentationEntry.Id }, newFermentationEntry);
        }

        // DELETE: api/FermentationEntries/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFermentationEntry(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var fermentationEntry = await _context.FermentationEntries.FindAsync(id);
            if (fermentationEntry == null)
            {
                return NotFound();
            }

            var wine = _context.Wines.Find(fermentationEntry.WineId);

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

            _context.FermentationEntries.Remove(fermentationEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FermentationEntryExists(Guid id)
        {
            return _context.FermentationEntries.Any(e => e.Id == id);
        }
    }
}

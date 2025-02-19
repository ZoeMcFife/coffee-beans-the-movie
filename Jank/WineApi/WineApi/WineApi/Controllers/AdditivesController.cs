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
    public class AdditivesController : ControllerBase
    {
        private readonly WineDbContext _context;
        private AuthHelper authHelper;

        public AdditivesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/Additives/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Additive>> GetAdditive(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var additive = await _context.Additives.Include(a => a.WineId).FirstOrDefaultAsync(a => a.Id == id);

            if (additive == null)
            {
                return NotFound();
            }

            var wineid = additive.WineId;

            if (wineid != null)
            {
                var wine = await _context.Wines.FindAsync(wineid);

                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            if (wineid == null)
            {
                return NotFound();
            }


            return additive;
        }

        // PUT: api/Additives/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAdditive(Guid id, Additive additive)
        {
            if (id != additive.Id)
            {
                return BadRequest();
            }

            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = _context.Wines.Find(additive.WineId);

            if (wine == null)
            {
                return NotFound();
            }

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            _context.Entry(additive).State = EntityState.Modified;

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
        public async Task<ActionResult<Additive>> PostAdditive(Additive additive)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wine = _context.Wines.Find(additive.WineId);

            if (wine == null)
            {
                return NotFound();
            }

            if (wine != null)
            {
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            var newAdditive = additive;
            _context.Additives.Add(newAdditive);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdditive", new { id = newAdditive.Id }, newAdditive);
        }

        // DELETE: api/Additives/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdditive(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
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
                if (wine.UserId != results.UserId)
                {
                    return Unauthorized();
                }
            }

            _context.Additives.Remove(additive);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdditiveExists(Guid id)
        {
            return _context.Additives.Any(e => e.Id == id);
        }
    }
}

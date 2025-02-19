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

namespace WineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WineTypesController : ControllerBase
    {
        private readonly WineDbContext _context;
        private AuthHelper authHelper = new AuthHelper();

        public WineTypesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/WineTypes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WineType>>> GetWineTypes()
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            return await _context.WineTypes.ToListAsync();
        }

        // GET: api/WineTypes/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<WineType>> GetWineType(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var wineType = await _context.WineTypes.FindAsync(id);

            if (wineType == null)
            {
                return NotFound();
            }

            return wineType;
        }

        // PUT: api/WineTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutWineType(Guid id, WineType wineType)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var userIsAdmin = await authHelper.UserHasAdminRights(_context, results.UserId);

            if (!userIsAdmin)
                return Unauthorized();

            if (id != wineType.Id)
            {
                return BadRequest();
            }

            _context.Entry(wineType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WineTypeExists(id))
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

        // POST: api/WineTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WineType>> PostWineType(WineType wineType)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var userIsAdmin = await authHelper.UserHasAdminRights(_context, results.UserId);

            if (!userIsAdmin)
                return Unauthorized();

            _context.WineTypes.Add(wineType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWineType", new { id = wineType.Id }, wineType);
        }

        // DELETE: api/WineTypes/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWineType(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var userIsAdmin = await authHelper.UserHasAdminRights(_context, results.UserId);

            if (!userIsAdmin)
                return Unauthorized();

            var wineType = await _context.WineTypes.FindAsync(id);
            if (wineType == null)
            {
                return NotFound();
            }

            _context.WineTypes.Remove(wineType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WineTypeExists(Guid id)
        {
            return _context.WineTypes.Any(e => e.Id == id);
        }
    }
}

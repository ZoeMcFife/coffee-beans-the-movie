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
    public class AdditiveTypesController : ControllerBase
    {
        private readonly WineDbContext _context;
        private AuthHelper authHelper = new AuthHelper();

        public AdditiveTypesController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/AdditiveTypes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AdditiveType>>> GetAdditiveTypes()
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            return await _context.AdditiveTypes.ToListAsync();
        }

        // GET: api/AdditiveTypes/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AdditiveType>> GetAdditiveType(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var additiveType = await _context.AdditiveTypes.FindAsync(id);

            if (additiveType == null)
            {
                return NotFound();
            }

            return additiveType;
        }

        // PUT: api/AdditiveTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAdditiveType(Guid id, AdditiveType additiveType)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var userIsAdmin = await authHelper.UserHasAdminRights(_context, results.UserId);

            if (!userIsAdmin)
                return Unauthorized();

            if (id != additiveType.Id)
            {
                return BadRequest();
            }

            _context.Entry(additiveType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdditiveTypeExists(id))
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

        // POST: api/AdditiveTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AdditiveType>> PostAdditiveType(AdditiveType additiveType)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var userIsAdmin = await authHelper.UserHasAdminRights(_context, results.UserId);

            if (!userIsAdmin)
                return Unauthorized();

            _context.AdditiveTypes.Add(additiveType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdditiveType", new { id = additiveType.Id }, additiveType);
        }

        // DELETE: api/AdditiveTypes/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdditiveType(Guid id)
        {
            var results = authHelper.GetAuthenticatedUser(this);

            if (!results.IsAuthenticated)
            {
                return results.ErrorResult;
            }

            var userIsAdmin = await authHelper.UserHasAdminRights(_context, results.UserId);

            if (!userIsAdmin)
                return Unauthorized();

            var additiveType = await _context.AdditiveTypes.FindAsync(id);
            if (additiveType == null)
            {
                return NotFound();
            }

            _context.AdditiveTypes.Remove(additiveType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdditiveTypeExists(Guid id)
        {
            return _context.AdditiveTypes.Any(e => e.Id == id);
        }
    }
}

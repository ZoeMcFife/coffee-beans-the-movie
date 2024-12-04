using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineApi.Context;
using WineApi.Model.DTO;

namespace WineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WineDbContext _context;

        public UsersController(WineDbContext context)
        {
            _context = context;
        }

        // GET: api/Users/Wines
        [HttpGet("{userId}/Wines")]
        public async Task<ActionResult<IEnumerable<WineDTO>>> GetWines(int userId)
        {
            return await _context.Wines
                .Select(w => WineDTO.MapWineToDto(w))
                .Where(w => w.UserId.Equals(userId))
                .ToListAsync();
        }
    }
}

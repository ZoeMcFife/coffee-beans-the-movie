using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WineApi.Context;
using WineApi.Model;
using WineApi.Model.DTO;

namespace WineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WineDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(WineDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Users/Wines
        [HttpGet("Wines")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WineDTO>>> GetWines()
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

            var wines = await _context.Wines
                .Where(w => w.UserId == userId)
                .ToListAsync();  // Retrieve the wines for the authenticated user

            var wineDtos = wines.Select(w => WineDTO.MapWineToDto(w)).ToList(); // Map to DTOs on the client side

            return Ok(wineDtos);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.Select(f => UserDTO.MapUserToDto(f)).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return UserDTO.MapUserToDto(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(UserRegisterDTO userDto)
        {
            // Check if username is already taken
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
            {
                return BadRequest("Username is already taken.");
            }

            // Hash the password
            var hashedPassword = HashPassword(userDto.Password);

            // Create a new User
            var newUser = new User
            {
                Username = userDto.Username,
                Password = hashedPassword,
                Email = userDto.Email
            };

            // Save to database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Return the newly created user's data
            return CreatedAtAction("GetUser", new { id = newUser.Id }, UserDTO.MapUserToDto(newUser));
        }

        // Helper function to hash passwords
        private string HashPassword(string password)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Use a key from configuration
            using var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        // Authentication endpoint
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Verify the password
            if (!VerifyPassword(userLogin.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token, User = UserDTO.MapUserToDto(user) });
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Use a key from configuration
            using var hmac = new HMACSHA256(key);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var computedHashString = Convert.ToBase64String(computedHash);

            return storedHash == computedHashString;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}

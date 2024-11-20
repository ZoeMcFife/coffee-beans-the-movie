using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{
    using global::Test.Model;
    using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class UserController : ControllerBase
        {
            [HttpGet]
            public IActionResult GetAllUsers()
            {
                return Ok();
            }

            [HttpPost("{userId}/{password}")]
            public IActionResult AuthorizeUser(int userId, string password)
            {
                return Ok();
            }


            [HttpGet("{id}")]
            public IActionResult GetUserById(int id)
            {
                return Ok();
            }

            [HttpPost]
            public IActionResult CreateUser()
            {
                return Ok();
            }

            [HttpPut("{id}")]
            public IActionResult UpdateUser(int id)
            {
                return Ok();
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteUser(int id)
            {
                return Ok();
            }
        }
    }

}

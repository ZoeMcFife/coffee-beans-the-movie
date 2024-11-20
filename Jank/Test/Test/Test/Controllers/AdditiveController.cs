using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    namespace Test.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AdditivesController : ControllerBase
        {
            [HttpGet]
            public IActionResult GetAllAdditives()
            {
                return Ok();
            }

            [HttpGet("{id}")]
            public IActionResult GetAdditiveById(int id)
            {
                return Ok();
            }

            [HttpPost]
            public IActionResult CreateAdditive()
            {
                return Ok();
            }

            [HttpPut("{id}")]
            public IActionResult UpdateAdditive(int id)
            {
                return Ok();
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteAdditive(int id)
            {
                return Ok();
            }
        }
    }

}

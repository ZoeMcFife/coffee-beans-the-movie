using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WineController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllWines()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetWineById(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateWine()
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateWine(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWine(int id)
        {
            return Ok();
        }
    }
}

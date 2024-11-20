using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FermentationController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllFermentations()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetFermentationById(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateFermentation()
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFermentation(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFermentation(int id)
        {
            return Ok();
        }
    }
}

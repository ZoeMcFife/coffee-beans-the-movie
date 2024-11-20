using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MostTreatmentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllTreatments()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetTreatmentById(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateTreatment()
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTreatment(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTreatment(int id)
        {
            return Ok();
        }
    }
}

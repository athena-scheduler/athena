using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers
{
    [Route("[controller]")]
    public class StudentController : AthenaControllerBase
    {
        [HttpGet]
        public IActionResult Profile() => View();

        [HttpGet("completed-courses")]
        public IActionResult ConfigureCompletedCourses() => View();
    }
}
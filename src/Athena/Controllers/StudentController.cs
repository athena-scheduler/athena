using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers
{
    [Route("[controller]")]
    public class StudentController : AthenaControllerBase
    {
        [HttpGet]
        public IActionResult Profile() => View();
        
        [HttpGet("welcome")]
        public IActionResult UserSetup(string returnUrl = null) => View("UserSetup", returnUrl);

        [HttpPost("welcome")]
        public IActionResult UserSetupComplete(string returnUrl = null) => RedirectToLocal(returnUrl);

        [HttpGet("completed-courses")]
        public IActionResult ConfigureCompletedCourses() => View();
    }
}
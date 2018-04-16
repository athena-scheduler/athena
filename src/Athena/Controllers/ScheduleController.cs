using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers
{
    [Route("[controller]")]
    public class ScheduleController : AthenaControllerBase
    {
        [HttpGet]
        public IActionResult ScheduleEditor() => View();

        [HttpGet("print")]
        public IActionResult PrintSchedule() => View();

    }
}
using Microsoft.AspNetCore.Mvc;

namespace Athena.Controllers
{
    [Route("[controller]")]
    public class ScheduleController : AthenaControllerBase
    {
        public IActionResult ScheduleEditor() => View();
    }
}
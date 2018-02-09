using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Athena.Models;

namespace Athena.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("Home")]
        public IActionResult Home()
        {
            return View();
        }
		
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Login_Page")]
        public IActionResult Login_Page()
        {
            return View();
        }

        [HttpGet("Add_Classes")]
        public IActionResult Add_Classes()
        {
            return View();
        }

        [HttpGet("Schedule_Page")]
        public IActionResult Schedule_Page()
        {
            return View();
        }

        [HttpGet("NewAccountPage")]
        public IActionResult NewAccountPage()
        {
            return View();
        }

        [HttpGet("Main_Page")]
        public IActionResult Main_Page()
        {
            return View();
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

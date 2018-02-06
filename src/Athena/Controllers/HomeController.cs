using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Athena.Models;

namespace Athena.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
		
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login_Page()
        {
            return View();
        }

        [HttpGet("About")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Schedule_Page()
        {
            return View();
        }

        public IActionResult NewAccountPage()
        {
            return View();
        }

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

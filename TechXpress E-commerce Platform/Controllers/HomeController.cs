using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechXpress.Data.Entities;
using TechXpress_E_commerce_Platform.Models;

namespace TechXpress_E_commerce_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitContact(ContactUs model)
        {
            if (ModelState.IsValid)
            {
                TempData["Success"] = "Your message has been sent successfully!";
                return RedirectToAction("Contact");
            }
            return View("Contact", model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

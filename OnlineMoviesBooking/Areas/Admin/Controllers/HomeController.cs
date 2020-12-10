using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineMoviesBooking.Models;

namespace OnlineMoviesBooking.Areas.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Error()
        {

            return View();
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Key") == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (HttpContext.Session.GetString("Key") != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }
            return View();
        }
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
    }
}

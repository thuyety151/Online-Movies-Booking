using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMoviesBooking.Areas.Controllers
{
    [Area("Admin")]
    public class DashBoardController : Controller
    {
       
        public IActionResult DashBoard()
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
    }
}

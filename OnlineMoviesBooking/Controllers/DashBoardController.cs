using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineMoviesBooking.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult DashBoard()
        {
            return View();
        }
    }
}

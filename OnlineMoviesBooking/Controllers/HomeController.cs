﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineMoviesBooking.Models;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Controllers
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
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                TempData["idlogin"] = HttpContext.Session.GetString("idLogin");
                TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
                TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
                TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            }
            else
            {
                HttpContext.Session.SetString("connectString", "Server=THANHTOAN\\SQLEXPRESS;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
            return View();
        }
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
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
        private readonly CinemaContext _context;
       
        public HomeController(ILogger<HomeController> logger, CinemaContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Login") != null)
            {
                string id = HttpContext.Session.GetString("Login").ToString();
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList();
                TempData["Logininf"] = account[0].Name;
                TempData["src"] = account[0].Image;
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

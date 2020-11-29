using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMoviesBooking.Areas.Customer.Controllers
{
    public class ShowsController : Controller
    {
        private readonly CinemaContext _context;
        private readonly ExecuteProcedure Exec;
        public ShowsController(CinemaContext context)
        {
            _context = context;
            Exec = new ExecuteProcedure(_context);
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}

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
    [Area("Customer")]
    public class MovieController : Controller
    {
        private readonly CinemaContext _context;
        private ExecuteProcedure Exec;
        public MovieController(CinemaContext context)
        {
            _context = context;
            Exec = new ExecuteProcedure(context);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NowShowing()
        {
            var movie = Exec.ExecuteGetMovieNow();
            return Json(new { data = movie });
        }
        public IActionResult Coming()
        {
            var movie = Exec.ExecuteGetMovieComingSoon();
            return Json(new { data = movie });
        }
        public IActionResult ComingSoon()
        {
            var movie = Exec.ExecuteGetMovieComingSoon();
            return View(movie);
        }
        public IActionResult Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var movie = Exec.ExecuteMovieDetail(id);
                return View(movie);
            }
            catch
            {
                return NotFound();
            }
        }
        public IActionResult ShowsDetail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theater = theater;

            
            List<ShowViewModel> lstshow = new List<ShowViewModel>();
            lstshow = Exec.ExecuteGetAllShowMovie(id);
            ViewBag.Movie = lstshow[0].MovieName;

            //if (show == null)
            //{
            //    return NotFound();
            //}

            return View(lstshow);
        }
        public IActionResult ShowsDate(DateTime date)
        {
            var show=Exec.ExecuteGetAllShowDate(date);
            return Json(new { data = show });
        }
    }
}

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
       
        public IActionResult ComingSoon()
        {
            var movie = Exec.ExecuteGetMovieComingSoon();
            return View(movie);
        }
        public IActionResult Detail(string id)
        {
            id = "c778815e-dc24-403d-a";
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

            ViewBag.Id = id;
            // rạp option
            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theater = theater;

            // date option
            DateTime now = DateTime.Now;
            int temp = 0;
            List<string> dateshow = new List<string>();
             while(temp!=7)
            {
                now =now.AddDays(1);
                dateshow.Add(now.Date.ToString("dd/MM/yyyy"));
                // now.Date.ToString("") + "/" + now.Date.ToString("MM") + "/" + now.Date.ToString("yyyy")
                temp++;
            }
            ViewBag.Date = dateshow;
            List<ShowViewModel> lstshow = new List<ShowViewModel>();
            lstshow = Exec.ExecuteGetAllShowMovie(id);

            // không có show
            if (lstshow.Count == 0)
            {
                ViewBag.Movie = Exec.ExecuteMovieDetail(id).Name;
            }
            else
            {
                ViewBag.Movie = lstshow[0].MovieName;
            }
            ViewBag.Show = lstshow;
            

            return View(lstshow);
        }


        //============================ Json
        public IActionResult ShowsDate(DateTime date)
        {
            var show = Exec.ExecuteGetAllShowDate(date);
            return Json(new { data = show });
        }
        public IActionResult Coming()
        {
            var movie = Exec.ExecuteGetMovieComingSoon();
            return Json(new { data = movie });
        }
        public IActionResult NowShowing()
        {
            var movie = Exec.ExecuteGetMovieNow();
            return Json(new { data = movie });
        }
        [HttpGet]
        public JsonResult getAllPost(int? page)
        {
            // search
            //var data = (from s in _db.Posts select s);
            //if (!String.IsNullOrEmpty(txtSearch))
            //{
            //    ViewBag.txtSearch = txtSearch;
            //    data = data.Where(s => s.Title.Contains(txtSearch));
            //}
            var movie = Exec.ExecuteGetMovieComingSoon();

      
            return Json(new { data = movie });
        }

        [HttpGet]
        public IActionResult getshowbydate(string idMovie, string date)
        {
            DateTime d = DateTime.Parse(date);
            var shows = Exec.ExecuteGetShowByDate(idMovie, d.ToString("yyyy-MM-dd"));
            // can co them ten rap
            return Json(  shows );
        }
    }
}

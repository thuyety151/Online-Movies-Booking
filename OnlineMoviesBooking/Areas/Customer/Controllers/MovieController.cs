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
        public IActionResult Now()
        {
            var movie = Exec.ExecuteGetMovieNow();
            return View("ComingSoon",movie);
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

            // tìm phim 
            var movie = Exec.ExecuteMovieDetail(id);

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
               
                dateshow.Add(now.Date.ToString("dd/MM/yyyy"));
                // now.Date.ToString("") + "/" + now.Date.ToString("MM") + "/" + now.Date.ToString("yyyy")
                now = now.AddDays(1);
                temp++;
            }
            ViewBag.Date = dateshow;
            
            return View(movie);
        }
        
        public IActionResult SeatPlan(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var plan = Exec.ExecuteGetDetailShow(id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewBag.IdShow = id;
            ViewBag.MovieName = plan.MovieName;
            ViewBag.ScreenName = plan.ScreenName;
            ViewBag.TheaterName = plan.TheaterName;
            ViewBag.TimeStart = plan.TimeStart.ToString("HH:mm");
            ViewBag.Date = plan.TimeStart.ToString("dd/mm/yyyy");

            var allseat = Exec.ExecGetAllSeat(plan.Id);
            ViewBag.RowA = allseat.Where(x => x.Row == "A").ToList();
            ViewBag.RowB = allseat.Where(x => x.Row == "B").ToList();
            ViewBag.RowC = allseat.Where(x => x.Row == "C").ToList();
            ViewBag.RowD = allseat.Where(x => x.Row == "D").ToList();
            ViewBag.RowE = allseat.Where(x => x.Row == "E").ToList();
            ViewBag.RowF = allseat.Where(x => x.Row == "F").ToList();
            ViewBag.RowG = allseat.Where(x => x.Row == "G").ToList();
            ViewBag.RowH = allseat.Where(x => x.Row == "H").ToList();

            return View();
        }
        [HttpGet]
        public IActionResult getinfo(string idshow,string lstSeat)
        {
            // Kiểm tra ghế và lịch hợp lệ => gán vào BillViewModel : nếu xác nhận bill sẽ add vào database
            // Kiểm tra ID show hợp lêk
            var show = Exec.ExecuteGetDetailShow(idshow);
            List<string> lstseat = lstSeat.Split(' ').ToList();
            if (show == null)
            {
                return Json(new {  success = false });
            }
            if (lstSeat == " undefined")
            {
                return Json(new { success = false });
            }
            foreach (var item in lstseat.ToList())
            {
                if(item== "undefined" || item == "")
                {
                    lstseat.Remove(item);
                }
            }
            if (lstseat.Count == 0)
            {
                return Json(new { success = false });
            }
            var seatVM = new List<Seat>();
            foreach (var item in lstseat)
            {
                seatVM.Add(Exec.ExecCheckIdSeat(item, idshow)); 
            }

            foreach (var item in seatVM)
            {
                if ( item.Id== null)
                {
                    return Json(new { success = false });
                }
            }

            var totalPrice = 0;
            foreach (var item in seatVM)
            {
                totalPrice+= Exec.FGetPrice(item.Id);
            }

            var bill = new
            {
                idShow = idshow,
                movieName=show.MovieName,  // movie
                seats=seatVM,
                theatername=show.TheaterName, //theater -- show.TheaterName
                screenname=show.ScreenName, //screen
                datestart=show.TimeStart.ToString("dd-MM-yyyy"),  //show    --
                timestart=show.TimeStart.ToString("HH:mm"),  //show    --
                totalprice=totalPrice, //seat    --
                languages=show.Languages

            };
            return Json( bill);
        }

        public IActionResult Checkout( string idshow,string bill)
        {
            if (idshow == null)
            {
                return NotFound();
            }
            var show = Exec.ExecuteGetDetailShow(idshow);
            if (show == null)
            {
                return NotFound();
            }

            return View();
        }
       // ============================ Json
        public IActionResult ShowsDate(DateTime date)
        {
            if(date==null)
            {
                return NotFound();
            }
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
            if(idMovie==null || date == null)
            {
                return NotFound();
            }
            DateTime d = DateTime.Parse(date);
            // can co them ten rap
            var theater = Exec.ExecuteFindTheaterShow(idMovie, d.ToString("yyyy-MM-dd"));
            // tìm tên, id các rạp thỏa điều kiện
            return Json(  theater );
        }
        [HttpGet]
        public IActionResult getprice()
        {
            //List<TypesOfSeat> price = new List<TypesOfSeat>();
             var price = Exec.GetAllTypesOfSeat();
            return Json(price);
        }
    }
}

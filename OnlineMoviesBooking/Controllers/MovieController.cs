using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMoviesBooking.Controllers
{
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
       
        public IActionResult ComingSoon(int? page)
        {
            List<Movie> lstmovie = new List<Movie>();
            int num = 2;
            var movieCount = Exec.GetCountMovieComing();
            if (movieCount % num == 0)
            {
                ViewBag.AllPage = movieCount / num;
            }
            else
            {
                ViewBag.AllPage = movieCount / num + 1;
            }
            // điều kiện để phân trang
            if (page != null && page > 0 && num < movieCount)
            {

                /*số trang có thể có:
                 *TH1: ví dụ có 18 sản phẩm, mỗi trang chứa 9 sản phẩm thì có TỐI ĐA 2 TRANG
                 *TH2: ví dụ có 20 sản phẩm, vậy có tối đa 3 trang và trang cuối chứa 2 sản phẩm 
               */
                int numpage = movieCount % num == 0 ? movieCount / num : (movieCount / num) + 1;

                if (page > numpage)
                {
                    ViewBag.page = numpage;
                }
                else
                {
                    ViewBag.page = page;
                }
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= numpage && movieCount % num != 0)
                {

                    // lấy số lẻ
                    lstmovie = (List<Movie>)Exec.ExecuteGetMovieComingSoon(num * (numpage - 1), movieCount % num);

                }
                //trường hợp 1
                else
                {

                    lstmovie = (List<Movie>)Exec.ExecuteGetMovieComingSoon(num * (page.GetValueOrDefault() - 1), num);
                }



            }
            else
            {
                ViewBag.page = 1;
                lstmovie = (List<Movie>)Exec.ExecuteGetMovieComingSoon(0, num);
            }
            return View(lstmovie);
        }
        public IActionResult Now(int? page)
        {
            List<Movie> lstmovie = new List<Movie>();
            int num = 2;
            var movieCount = Exec.GetCountMovieNow();
            if (movieCount % num == 0)
            {
                ViewBag.AllPage = movieCount / num;
            }
            else
            {
                ViewBag.AllPage = movieCount / num + 1;
            }
            // điều kiện để phân trang
            if (page != null && page > 0 && num < movieCount)
            {

                /*số trang có thể có:
                 *TH1: ví dụ có 18 sản phẩm, mỗi trang chứa 9 sản phẩm thì có TỐI ĐA 2 TRANG
                 *TH2: ví dụ có 20 sản phẩm, vậy có tối đa 3 trang và trang cuối chứa 2 sản phẩm 
               */
                int numpage = movieCount % num == 0 ? movieCount / num : (movieCount / num) + 1;

                if (page > numpage)
                {
                    ViewBag.page = numpage;
                }
                else
                {
                    ViewBag.page = page;
                }
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= numpage && movieCount % num != 0)
                {

                    // lấy số lẻ
                     lstmovie = (List<Movie>)Exec.ExecuteGetMovieNow(num * (numpage - 1), movieCount%num);

                }
                //trường hợp 1
                else
                {

                     lstmovie = (List<Movie>)Exec.ExecuteGetMovieNow(num * (page.GetValueOrDefault() - 1), num);
                }



            }
            else
            {
                ViewBag.page = 1;
                 lstmovie = (List<Movie>)Exec.ExecuteGetMovieNow(0, num);
            }
            return View( lstmovie);
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
        [HttpGet]
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
        [HttpGet]
        public IActionResult TimeOut()
        {
            return RedirectToAction("Index");
        }
    }
}

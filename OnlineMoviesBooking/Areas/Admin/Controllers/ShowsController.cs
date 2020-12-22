using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModels;

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShowsController : Controller
    {
        private readonly ExecuteProcedure Exec;
        private readonly string check;
        public ShowsController(IHttpContextAccessor httpContextAccessor)
        {
            Exec = new ExecuteProcedure(httpContextAccessor.HttpContext.Session.GetString("connectString"));
            string username = httpContextAccessor.HttpContext.Session.GetString("idLogin");
            string connectionString = httpContextAccessor.HttpContext.Session.GetString("connectString");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = $"EXEC dbo.USP_CheckAdmin @username = '{username}' ";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        check = Convert.ToString(reader[0]);
                    }
                }
                catch (SqlException e)
                {
                    connection.Close();
                    check = "0";
                }
                connection.Close();
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            var shows=Exec.ExecuteGetAllShow().Select(x=> new {
                movieName=x.MovieName,
                poster=x.Poster,
                timeStart=x.TimeStart.ToShortTimeString() +"\n"+ x.TimeStart.ToShortDateString(),
                screenName=x.ScreenName,
                theaterName=x.TheaterName,
                id=x.Id
            });
            return Json(new { data = shows });
        }
        public IActionResult Search(string id)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            if (id == null)
            {
                var obja = Exec.ExecuteGetAllShow();
                return Json(new { data = obja });
            }
            // tìm cáp phòng chiếu theo rạp
            var obj = Exec.ExecuteGetAllShowTheater(id);
            return Json(new { data = obj });
        }
        public IActionResult SearchStatus(string status)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            if (status == "true")   // chưa chiếu
            {
                var obju = Exec.ExecuteGetAllShowisComing();
                return Json(new { data = obju });
            }
            else if (status == "false")     // đã chiếu
            {
                var objn = Exec.ExecuteGetAllShowisUsed();
                return Json(new { data = objn });
            }
            // tìm cáp phòng chiếu theo rạp
            var obja = Exec.ExecuteGetAllShow();
            return Json(new { data = obja });
        }
        public IActionResult Index()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theater = new SelectList(theater, "Id", "Name");
            return View();
        }

        public IActionResult Details(string id)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            ShowViewModel show = Exec.ExecuteGetDetailShow(id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }
        [HttpGet]
        public IActionResult GetTheater()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            var theater = Exec.ExecuteTheaterGetAll();
            return Json(new { data = theater });
        }
        [HttpGet]
        public IActionResult GetScreen(string id)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            var screen = Exec.SearchScreenwithTheater(id);
            var obj = screen.Select(x => new
            {
                Id=x.Id,
                Text=x.Name
            });
            return Json( new { items= obj });
        }

        public IActionResult Create()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            var movies = Exec.ExecuteMovieGetAll();
            ViewBag.Movies = new SelectList(movies, "Id", "Name");

            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theaters = new SelectList(theater, "Id", "Name");

            var screen = Exec.SearchScreenwithTheater(theater[0].Id);
            ViewBag.Screens = new SelectList(screen, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Show showVM)
        {

            if (ModelState.IsValid)
            {
                showVM.Id = Guid.NewGuid().ToString();
                if(showVM.Languages==null)
                {
                    showVM.Languages = "";
                }    
                // Chuyển số phút sang hh:mm:ss
                TimeSpan ts = TimeSpan.FromMinutes(114);

                string s=Exec.ExecuteInsertShow(showVM);
                if(s.Contains("Ngày giờ không hợp lệ"))
                {
                    ModelState.AddModelError("TimeStart", "Ngày giờ không hợp lệ");
                }
                else if (s.Contains("Trùng lịch chiếu"))
                {
                    // show trigger error
                    ModelState.AddModelError("TimeStart", "Trùng lịch chiếu");
                }
                else if(s.Contains("Giờ không hợp lệ"))
                {
                    ModelState.AddModelError("TimeStart", "Giờ không hợp lệ");
                } 
                
                // có lỗi catch từ trigger
                if(ModelState.ErrorCount==0)
                {
                    return RedirectToAction(nameof(Index));
                }    
            }
            // modelstate.valid== false 
            // modelstate.errorcount>0
            var movies = Exec.ExecuteMovieGetAll();
            ViewBag.Movies = new SelectList(movies, "Id", "Name");

            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theaters = new SelectList(theater, "Id", "Name");

            var screen = Exec.ExecuteScreenGetAllwithTheater();
            ViewBag.Screens = new SelectList(screen, "Id", "Name");
            
            return View(showVM);
        }

        public IActionResult Edit(string id)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            var show = Exec.ExecuteGetDetailShowEdit(id);
            if (show == null)
            {
                return NotFound();
            }

            var movies = Exec.ExecuteMovieGetAll();
            ViewBag.Movies = new SelectList(movies, "Id", "Name");

            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theaters = new SelectList(theater, "Id", "Name");

            var screen = Exec.ExecuteScreenGetAllwithTheater();
            ViewBag.Screens = new SelectList(screen, "Id", "Name");

            return View(show);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, ShowViewModel show)
        {
            if (id != show.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(show.Languages==null)
                {
                    show.Languages = "";
                }    
                string s = Exec.ExecuteUpdateShow(show);
                if (s.Contains("Trùng lịch chiếu"))
                {
                    // show trigger error
                    ModelState.AddModelError("TimeStart", "Trùng lịch chiếu");
                }
                else if (s.Contains("Giờ không hợp lệ á"))
                {
                    ModelState.AddModelError("TimeStart", "Giờ không hợp lệ");
                }

                // có lỗi catch từ trigger
                if (ModelState.ErrorCount == 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            var movies = Exec.ExecuteMovieGetAll();
            ViewBag.Movies = new SelectList(movies, "Id", "Name");

            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theaters = new SelectList(theater, "Id", "Name");

            var screen = Exec.ExecuteScreenGetAllwithTheater();
            ViewBag.Screens = new SelectList(screen, "Id", "Name");

            return View(show);
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            Exec.ExecuteDeleteShow(id);
            return Json(new { success = true });
        }

    }
}

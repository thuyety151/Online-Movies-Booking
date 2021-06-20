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
    public class ScreensController : Controller
    {
        private ExecuteProcedure Exec;
        private readonly string check;

        public ScreensController(IHttpContextAccessor httpContextAccessor)
        {
            Exec = new ExecuteProcedure(httpContextAccessor.HttpContext.Session.GetString("connectString").ToString());        // sao chỗ này lỗi v
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
                check = "1";
                connection.Close();
            }
        }
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
            var obj=Exec.ExecuteScreenGetAllwithTheater();
            return Json(new {data=obj});
        }
        // GET: Screens
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
            ViewBag.Theater= new SelectList(theater, "Id", "Name");
            return View();
        }

        // GET: Screens/Details/5
        [HttpGet]
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

            var screen = Exec.ExecuteGetDetailScreen_Theater(id);
            if (screen == null)
            {
                return NotFound();
            }
           
            return View(screen);
        }

        // GET: Screens/Create
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
            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theater = new SelectList(theater, "Id", "Name");
          
            return View();
        }

        // POST: Screens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,IdTheater")] Screen screen)
        {
            if (ModelState.IsValid)
            {
                screen.Id = Guid.NewGuid().ToString("N").Substring(0, 10);
                string checkname = Exec.CheckNameScreen(screen.Name, screen.IdTheater);
                // check lỗi do nhập
                if (checkname != "") 
                {
                    ModelState.AddModelError("Name", checkname);
                }
                else
                {
                    // check lỗi do add dưới db
                    string s = Exec.ExecuteInsertScreen(screen);

                    while (s.Contains("PRIMARY"))
                    {
                        screen.Id= Guid.NewGuid().ToString("N").Substring(0, 10);
                        s = Exec.ExecuteInsertScreen(screen);
                    }

                    // transaction
                    if (s!="")
                    {
                        ModelState.AddModelError("Name", s);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theater = new SelectList(theater, "Id", "Name");
            return View(screen);
        }

        // GET: Screens/Edit/5
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

            var screen = Exec.ExecuteGetDetailScreen_Theater(id);
            
            if (screen == null)
            {
                return NotFound();
            }
            Screen obj = new Screen
            {
                Id = screen.Id,
                Name = screen.Name,
                IdTheater = screen.IdTheater
            };
            ViewBag.Theater = new SelectList(Exec.ExecuteTheaterGetAll(), "Id", "Name");
            return View(obj);
        }

        // POST: Screens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,IdTheater")] Screen screen)
        {
            if (id != screen.Id)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    string checkname = Exec.CheckNameScreen(screen.Name, screen.IdTheater);
                    // check lỗi do nhập
                    if (checkname != "")
                    {
                        ModelState.AddModelError("Name", "Tên đã tồn tại");
                    }
                    Exec.ExecuteUpdateScreen(screen);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {

            }
            ViewBag.Theater = new SelectList(Exec.ExecuteTheaterGetAll(), "Id", "Name");
            return View(screen);
        }

       
        // POST: Screens/Delete/5
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
            string result=Exec.ExecuteDeleteScreen(id);
             if(result!="")
                {
                return Json(new { success = result });
                }
            return Json(new { success = true });
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
            if (id==null)
            {
                var obja = Exec.ExecuteScreenGetAllwithTheater();
                return Json(new { data = obja });
            }    
            // tìm cáp phòng chiếu theo rạp
            var obj = Exec.SearchScreenwithTheater(id);
            return Json(new { data = obj });
        }

    }
}

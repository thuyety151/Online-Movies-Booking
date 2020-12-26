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

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TheatersController : Controller
    {
        private ExecuteProcedure Exec;
        private readonly string check;
        public TheatersController(IHttpContextAccessor httpContextAccessor)
        {
            Exec = new ExecuteProcedure(httpContextAccessor.HttpContext.Session.GetString("connectString").ToString());
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

        public JsonResult GetAll()
        {

            var obj = Exec.ExecuteTheaterGetAll().Select(x=>new { 
                id=x.Id,
                name=x.Name,
                address=x.Address,
                hotline=x.Hotline
            });
            
            return Json(new { data = obj });
        }
        // GET: Theaters
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
            return View();
        }

        // GET: Theaters/Details/5
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
            // chi tiết rạp chiếu gồm: danh sách các phòng chiếu
            if (id == null)
            {
                return NotFound();
            }
            var screen= Exec.SearchScreenwithTheater(id);
            var view = "";
            foreach (var item in screen)
            {
                view += item.Name + "\n";
            }
            ViewBag.Screen = view;
            var theater = Exec.ExecuteDetailTheater(id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // GET: Theaters/Create
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("Id,Name,Address,Hotline")] Theater theater)
        {
            theater.Id = Guid.NewGuid().ToString("N").Substring(0, 10);
            if (ModelState.IsValid)
            {
                
                string s= Exec.ExecuteInsertTheater(theater.Id, theater.Name, theater.Address, theater.Hotline);

                while (s.Contains("PRIMARY"))   // do check primary key trước
                {
                    theater.Id = Guid.NewGuid().ToString("N").Substring(0, 10);
                    s = Exec.ExecuteInsertTheater(theater.Id, theater.Name, theater.Address, theater.Hotline);
                }
                if (s.Contains("UNIQUE"))       //check unique address
                {
                    // có error message
                    ModelState.AddModelError("Address", s);
                    return View(theater);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(theater);
        }

        // GET: Theaters/Edit/5
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
            var theater = Exec.ExecuteDetailTheater(id);


            if (theater == null)
            {
                return NotFound();
            }
            return View(theater);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,Address,Hotline")] Theater theater)
        {
            if (id != theater.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string result = Exec.ExecuteUpdateTheater(theater);
                if (result.Contains("UNIQUE"))       //check unique address
                {
                    // có error message
                    ModelState.AddModelError("Address", result);
                    return View(theater);
                }
                return RedirectToAction(nameof(Index));
                
            }
            return View(theater);
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
            string s=Exec.ExecuteDeleteTheater(id);
            if (s != "")
            {
                return Json(new { success = s });
            }
            return Json(new { success = true });
        }


    }
}

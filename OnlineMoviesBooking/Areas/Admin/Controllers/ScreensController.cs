using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModels;

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScreensController : Controller
    {
        private readonly ExecuteProcedure Exec;

        public ScreensController()
        {
            Exec = new ExecuteProcedure();
        }
        public IActionResult GetAll()
        {
            var obj=Exec.ExecuteScreenGetAllwithTheater();
            return Json(new {data=obj});
        }
        // GET: Screens
        public IActionResult Index()
        {
            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theater= new SelectList(theater, "Id", "Name");
            return View();
        }

        // GET: Screens/Details/5
        [HttpGet]
        public IActionResult Details(string id)
        {
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
                    ModelState.AddModelError("Name", "Tên đã tồn tại");
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
                    if (s == "2627")
                    {
                        ModelState.AddModelError("Name", "Có lỗi xảy ra");
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
            string result=Exec.ExecuteDeleteScreen(id);
            if (result == "2627")
            {
                return Json(new { success = false });
            }
            else if(result== "Phòng chiếu đang có lịch chiếu")
            {
                return Json(new { success = false, message= "Phòng chiếu đang có lịch chiếu" });
                }
            return Json(new { success = true });
        }
        public IActionResult Search(string id)
        {
            if(id==null)
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

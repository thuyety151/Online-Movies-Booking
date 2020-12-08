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
        private readonly CinemaContext _context;
        private ExecuteProcedure Exec;

        public ScreensController(CinemaContext context)
        {
            _context = context;
            Exec = new ExecuteProcedure(_context);
        }
        public IActionResult GetAll()
        {
            var obj=Exec.ExecuteScreenGetAllwithTheater();
            return Json(new {data=obj});
        }
        // GET: Screens
        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> Create([Bind("Id,Name,IdTheater")] Screen screen)
        {
            if (ModelState.IsValid)
            {
                screen.Id = Guid.NewGuid().ToString();
                if (Exec.CheckNameScreen(screen.Name, screen.IdTheater) > 0)
                {
                    ModelState.AddModelError("Name", "Tên đã tồn tại");
                }
                else
                {
                    string s=Exec.ExecuteInsertScreen(screen);
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
        public async Task<IActionResult> Edit(string id)
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,IdTheater")] Screen screen)
        {
            if (id != screen.Id)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (Exec.CheckNameScreen(screen.Name, screen.IdTheater) > 0)
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
            Exec.ExecuteDeleteScreen(id);
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
        private bool ScreenExists(string id)
        {
            return _context.Screen.Any(e => e.Id == id);
        }
    }
}

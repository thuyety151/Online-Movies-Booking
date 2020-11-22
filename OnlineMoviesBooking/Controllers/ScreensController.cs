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

namespace OnlineMoviesBooking.Controllers
{
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
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screen = await _context.Screen
                .Include(s => s.IdTheaterNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            try
            {
                if (ModelState.IsValid)
                {
                    screen.Id = Guid.NewGuid().ToString();
                    if (Exec.CheckNameScreen(screen.Name, screen.IdTheater) > 0)
                    {
                        ModelState.AddModelError("Name", "Tên đã tồn tại");
                    }
                    Exec.ExecuteInsertScreen(screen);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {

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

            var screen = await _context.Screen.FindAsync(id);
            if (screen == null)
            {
                return NotFound();
            }
            ViewData["IdTheater"] = new SelectList(_context.Theater, "Id", "Id", screen.IdTheater);
            return View(screen);
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(screen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreenExists(screen.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTheater"] = new SelectList(_context.Theater, "Id", "Id", screen.IdTheater);
            return View(screen);
        }

        // GET: Screens/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screen = await _context.Screen
                .Include(s => s.IdTheaterNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screen == null)
            {
                return NotFound();
            }

            return View(screen);
        }

        // POST: Screens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var screen = await _context.Screen.FindAsync(id);
            _context.Screen.Remove(screen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreenExists(string id)
        {
            return _context.Screen.Any(e => e.Id == id);
        }
    }
}

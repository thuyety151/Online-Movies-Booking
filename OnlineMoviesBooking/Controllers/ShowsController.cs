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
    public class ShowsController : Controller
    {
        private readonly CinemaContext _context;
        private readonly ExecuteProcedure Exec;
        public ShowsController(CinemaContext context)
        {
            _context = context;
            Exec = new ExecuteProcedure(_context);
        }

        // GET: Shows
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.Show.Include(s => s.IdMovieNavigation).Include(s => s.IdScreenNavigation);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: Shows/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Show
                .Include(s => s.IdMovieNavigation)
                .Include(s => s.IdScreenNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }
        [HttpGet]
        public IActionResult GetTheater()
        {
            var theater = Exec.ExecuteTheaterGetAll();
            return Json(new { data = theater });
        }
        [HttpGet]
        public IActionResult GetScreen(string id)
        {
            var screen = Exec.SearchScreenwithTheater(id);
            var obj = screen.Select(x => new
            {
                Id=x.Id,
                Text=x.Name
            });
            return Json( new { items= obj });
        }

        // GET: Shows/Create
        public IActionResult Create()
        {
            var movies = Exec.ExecuteMovieGetAll();
            ViewBag.Movies = new SelectList(movies, "Id", "Name");

            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theaters = new SelectList(theater, "Id", "Name");

            var screen = Exec.ExecuteScreenGetAllwithTheater();
            ViewBag.Screens = new SelectList(screen, "Id", "Name");
            return View();
        }

        // POST: Shows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Show showVM)
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

                if (s.Contains("Trùng lịch chiếu"))
                {
                    // show trigger error
                    ModelState.AddModelError("Screen","Trùng lịch chiếu");
                }
                else if(s.Contains("Giờ không hợp lệ á"))
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

        // GET: Shows/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Show.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            ViewData["IdMovie"] = new SelectList(_context.Movie, "Id", "Id", show.IdMovie);
            ViewData["IdScreen"] = new SelectList(_context.Screen, "Id", "Id", show.IdScreen);
            return View(show);
        }

        // POST: Shows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Languages,TimeStart,TimeEnd,IdMovie,IdScreen")] Show show)
        {
            if (id != show.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(show);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowExists(show.Id))
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
            ViewData["IdMovie"] = new SelectList(_context.Movie, "Id", "Id", show.IdMovie);
            ViewData["IdScreen"] = new SelectList(_context.Screen, "Id", "Id", show.IdScreen);
            return View(show);
        }

        // GET: Shows/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Show
                .Include(s => s.IdMovieNavigation)
                .Include(s => s.IdScreenNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        // POST: Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var show = await _context.Show.FindAsync(id);
            _context.Show.Remove(show);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowExists(string id)
        {
            return _context.Show.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Controllers
{
    public class QasController : Controller
    {
        private readonly CinemaContext _context;

        public QasController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Qas
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.Qa.Include(q => q.IdAccountNavigation);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: Qas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qa = await _context.Qa
                .Include(q => q.IdAccountNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qa == null)
            {
                return NotFound();
            }

            return View(qa);
        }

        // GET: Qas/Create
        public IActionResult Create()
        {
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Name");
            return View();
        }

        // POST: Qas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAccount,Email,Time,Content")] Qa qa)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(qa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id", qa.IdAccount);
            return View(qa);
        }

        // GET: Qas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qa = await _context.Qa.FindAsync(id);
            if (qa == null)
            {
                return NotFound();
            }
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id", qa.IdAccount);
            return View(qa);
        }

        // POST: Qas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,IdAccount,Email,Time,Content")] Qa qa)
        {
            if (id != qa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QaExists(qa.Id))
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
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id", qa.IdAccount);
            return View(qa);
        }

        // POST: Qas/Delete/5
        [HttpDelete]
        public IActionResult DeleteConfirmed(string id)
        {
            try
            {
                _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_DeleteQuestionAnswer @id = {id}");
                return Json(new { success = true, message = "Xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool QaExists(string id)
        {
            return _context.Qa.Any(e => e.Id == id);
        }
    }
}

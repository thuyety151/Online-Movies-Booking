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
    public class TypesOfSeatsController : Controller
    {
        private readonly CinemaContext _context;

        public TypesOfSeatsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: TypesOfSeats
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypesOfSeat.ToListAsync());
        }

        // GET: TypesOfSeats/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typesOfSeat = await _context.TypesOfSeat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typesOfSeat == null)
            {
                return NotFound();
            }

            return View(typesOfSeat);
        }

        // GET: TypesOfSeats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypesOfSeats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Cost")] TypesOfSeat typesOfSeat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typesOfSeat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typesOfSeat);
        }

        // GET: TypesOfSeats/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typesOfSeat = await _context.TypesOfSeat.FindAsync(id);
            if (typesOfSeat == null)
            {
                return NotFound();
            }
            return View(typesOfSeat);
        }

        // POST: TypesOfSeats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Cost")] TypesOfSeat typesOfSeat)
        {
            if (id != typesOfSeat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typesOfSeat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypesOfSeatExists(typesOfSeat.Id))
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
            return View(typesOfSeat);
        }

        // GET: TypesOfSeats/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typesOfSeat = await _context.TypesOfSeat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typesOfSeat == null)
            {
                return NotFound();
            }

            return View(typesOfSeat);
        }

        // POST: TypesOfSeats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var typesOfSeat = await _context.TypesOfSeat.FindAsync(id);
            _context.TypesOfSeat.Remove(typesOfSeat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypesOfSeatExists(string id)
        {
            return _context.TypesOfSeat.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Controllers
{
    public class TypesOfSeatsController : Controller
    {
        private readonly CinemaContext _context;
        private ExecuteProcedure Exec;

        public TypesOfSeatsController(CinemaContext context)
        {
            _context = context;
            Exec = new ExecuteProcedure(_context);
        }


        // GET: TypesOfSeats
        public IActionResult Index()
        {
            var types = Exec.GetAllTypesOfSeat();
            return View(types);
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
                if (Exec.CheckToSeatName(typesOfSeat.Id, typesOfSeat.Name) > 0)
                {
                    ModelState.AddModelError("Name", "Tên đã tồn tại");
                    return View(typesOfSeat);
                }
                Exec.ExecuteUpdateTypesOfSeat(typesOfSeat);
                return RedirectToAction(nameof(Index));
            }
            return View(typesOfSeat);
        }
        
        private bool TypesOfSeatExists(string id)
        {
            return _context.TypesOfSeat.Any(e => e.Id == id);
        }
    }
}

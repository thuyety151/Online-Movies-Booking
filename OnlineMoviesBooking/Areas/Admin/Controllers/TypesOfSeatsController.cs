using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TypesOfSeatsController : Controller
    {
        private ExecuteProcedure Exec;

        public TypesOfSeatsController(IHttpContextAccessor httpContextAccessor)
        {
            Exec = new ExecuteProcedure(httpContextAccessor.HttpContext.Session.GetString("connectString"));
        }


        // GET: TypesOfSeats
        public IActionResult Index()
        {
            var types = Exec.GetAllTypesOfSeat();
            return View(types);
        }

        // GET: TypesOfSeats/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typesOfSeat = Exec.ExecGetDetailTypeOfSeat(id);
            if (typesOfSeat == null)
            {
                return NotFound();
            }

            return View(typesOfSeat);
        }


        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typesOfSeat = Exec.ExecGetDetailTypeOfSeat(id);
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
        
       
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TheatersController : Controller
    {
        private readonly CinemaContext _context;
        private ExecuteProcedure Exec;
        public TheatersController(CinemaContext context)
        {
            _context = context;
            Exec = new ExecuteProcedure(context);
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
            return View();
        }

        // GET: Theaters/Details/5
        public async Task<IActionResult> Details(string id)
        {
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("Id,Name,Address,Hotline")] Theater theater)
        {
            theater.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                // chưa đưa ra được trigger execption
                string s= Exec.ExecuteInsertTheater(theater.Id, theater.Name, theater.Address, theater.Hotline);
                if(s!="")
                {
                    // có error message
                    ModelState.AddModelError("Address", "Địa chỉ đã tồn tại");
                    return View(theater);
                }    
                return RedirectToAction(nameof(Index));
            }
            return View(theater);
        }

        // GET: Theaters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theater = await _context.Theater.FindAsync(id);
            if (theater == null)
            {
                return NotFound();
            }
            return View(theater);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Address,Hotline")] Theater theater)
        {
            if (id != theater.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theater);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheaterExists(theater.Id))
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
            return View(theater);
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            Exec.ExecuteDeleteTheater(id);
            return Json(new { success = true });
        }

        private bool TheaterExists(string id)
        {
            return _context.Theater.Any(e => e.Id == id);
        }
    }
}

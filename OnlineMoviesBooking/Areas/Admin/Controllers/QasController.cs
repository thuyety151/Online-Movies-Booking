using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Areas.Controllers
{
    [Area("Admin")]
    public class QasController : Controller
    {
        private readonly CinemaContext _context;

        public QasController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Qas
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Key") == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (HttpContext.Session.GetString("Key") != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }
            var cinemaContext = _context.Qa.FromSqlRaw("EXEC dbo.USP_GetAllQa");
            return View(cinemaContext);
        }


        public IActionResult Get(string id)
        {
            if (HttpContext.Session.GetString("Key") == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (HttpContext.Session.GetString("Key") != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }
            try
            {
                var qa = _context.Qa.FromSqlRaw($"EXEC dbo.USP_GetQa @id = '{id}'");    // 
                return Json(new { data = qa });
            }
            catch (Exception e)
            {

                return Json(new { data = e.Message });


            }
        }

        // GET: Qas/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Key") == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (HttpContext.Session.GetString("Key") != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }
            //ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id");
            return View();
        }

        // POST: Qas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Email,Content")] Qa qa)
        {
            if (ModelState.IsValid)
            {
                qa.Id = Guid.NewGuid().ToString();
                qa.Time = DateTime.Now;
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetAccountForEmail @Email = '{qa.Email}'").ToList();
                qa.IdAccount = account[0].Id;
                _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertQa @id = {qa.Id}, @idaccount = {qa.IdAccount},@email = {qa.Email}, @time = {qa.Time}, @content ={qa.Content} ");
                return RedirectToAction(nameof(Index));
            }
            return View(qa);
        }

        // GET: Qas/Edit/5
        public IActionResult Edit(string id)
        {
            if (HttpContext.Session.GetString("Key") == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (HttpContext.Session.GetString("Key") != "Admin")
            {
                return RedirectToAction("Error", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var qa = _context.Qa.FromSqlRaw($"EXEC dbo.USP_GetQa @id = '{id}'").ToList();
            if (qa == null)
            {
                return NotFound();
            }
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id", qa[0].IdAccount);
            return View(qa[0]);
        }

        // POST: Qas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,IdAccount,Email,Time,Content")] Qa qa)
        {
            if (id != qa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_UpdateQA @id = {qa.Id}, @time = {qa.Time}, @content = {qa.Content}");
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
        public IActionResult Delete(string id)
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

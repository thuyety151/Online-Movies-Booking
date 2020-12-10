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
    public class TypeOfMembersController : Controller
    {
        private readonly CinemaContext _context;

        public TypeOfMembersController(CinemaContext context)
        {
            _context = context;
        }

        // GET: TypeOfMembers
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
            var list = _context.TypeOfMember.FromSqlRaw($"EXEC dbo.USP_GetAllMemberType");
            return View(list);
        }

        

        // GET: TypeOfMembers/Create
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
            return View();
        }

        // POST: TypeOfMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TypeOfMember typeOfMember)
        {
            if (ModelState.IsValid)
            {
                _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertMemberType @id = {typeOfMember.IdTypeMember},  @Name = {typeOfMember.TypeOfMemberName}, @content = {typeOfMember.Content},@money={typeOfMember.Money}, @point = {typeOfMember.Point}");
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfMember);
        }

        // GET: TypeOfMembers/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfMember =  _context.TypeOfMember.FromSqlRaw($"EXEC dbo.USP_GetMemberType @id = '{id}'").ToList();
            if (typeOfMember == null)
            {
                return NotFound();
            }
            return View(typeOfMember[0]);
        }

        // POST: TypeOfMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,TypeOfMember typeOfMember)
        {
            if (id != typeOfMember.IdTypeMember)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_UpdateMemberType @id = {typeOfMember.IdTypeMember},  @Name = {typeOfMember.TypeOfMemberName}, @content = {typeOfMember.Content},@money = {typeOfMember.Money}, @point = {typeOfMember.Point}");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfMemberExists(typeOfMember.IdTypeMember))
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
            return View(typeOfMember);
        }
        // POST: TypeOfMembers/Delete/5
        [HttpDelete]
        
        public IActionResult Delete(string id)
        {
            _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_DeleteMemberType @id = {id}");
            return Json(new { success = true,  mesage = "Xóa thành công"});
        }

        private bool TypeOfMemberExists(string id)
        {
            return _context.TypeOfMember.Any(e => e.IdTypeMember == id);
        }
    }
}

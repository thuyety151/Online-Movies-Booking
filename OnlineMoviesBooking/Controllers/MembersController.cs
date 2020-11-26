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
    public class MembersController : Controller
    {
        private readonly CinemaContext _context;

        public MembersController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var list = _context.Account.FromSqlRaw("EXEC dbo.USP_SelectAllMember").ToList();
            return View(list);
        }

        // GET: Members/Details/5
        public IActionResult Details(string id)
        {
            try
            {
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'");    // 
                return Json(new { data = account });
            }
            catch (Exception e)
            {

                return Json(new { data = e.Message });


            }
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Birthdate,Gender,Address,Sdt,Email,Password,Point,IdTypesOfUser,Image")] Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    account.Id = Guid.NewGuid().ToString();
                    account.IdTypesOfUser = "2";
                    _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertUpdateAccount @id = {account.Id},@name = {account.Name},@birthdate = {account.Birthdate},@gender={account.Gender},@address={account.Address},@SDT={account.Sdt},@Email={account.Email},@password={account.Password},@point ={account.Point},@usertypeid={account.IdTypesOfUser},@image={account.Image},@action ={"Insert"} ");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                var modelstateKey = ModelState.Keys;
                var list = _context.Account.FromSqlRaw($"EXEC dbo.USP_CheckEmail '{account.Email}' ").ToList();
                if (list.Count() == 1)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                }
                //else if (check_db == 1)
                //{
                //    ModelState.AddModelError("Sdt", "Sdt đã tồn tại");
                //}
                return View();
            }
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
            return View(account);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
            return View(account);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Birthdate,Gender,Address,Sdt,Email,Password,Point,IdTypesOfUser,Image")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertUpdateAccount @id = {account.Id},@name = {account.Name},@birthdate = {account.Birthdate},@gender={account.Gender},@address={account.Address},@SDT={account.Sdt},@Email={account.Email},@password={account.Password},@point ={account.Point},@usertypeid={account.IdTypesOfUser},@image={account.Image},@action ={"Update"} ");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
            return View(account);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_DeleteMember @id = {id}");
                return Json(new { success = true, message = "Xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }


        }

        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}

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
    public class AccountsController : Controller
    {
        private readonly CinemaContext _context;

        public AccountsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            //var cinemaContext = _context.Account.Include(a => a.IdTypesOfUserNavigation);
            
            var list = _context.Account.FromSqlRaw("EXEC dbo.USP_SelectAllAccount").ToList();
            return View( list);
        }

        //public IActionResult GetAll()
        //{
        //    var list = _context.Account.FromSqlRaw("EXEC dbo.SelectAccount").ToList();
        //    return Json(new { data = list });
        //}
        // GET: Accounts/Details/5
        [HttpGet]
        public IActionResult Get(string id)
        {
            var account = _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_GetDetailAccount @id = {id}");
            return Json(new { data = account });
        }
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = await _context.Account
        //        .Include(a => a.IdTypesOfUserNavigation)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(account);
        //}

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id");
            return View();
        }

        // POST: Accounts/Create
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

                    _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertUpdateAccount @id = {account.Id},@name = {account.Name},@birthdate = {account.Birthdate},@gender={account.Gender},@address={account.Address},@SDT={account.Sdt},@Email={account.Email},@password={account.Password},@point ={account.Point},@usertypeid={account.IdTypesOfUser},@image={account.Image},@action ={"Insert"} ");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
               var modelstateKey =  ModelState.Keys;
                var list = _context.Account.FromSqlRaw($"EXEC dbo.USP_CheckEmail '{account.Email}' ").ToList();
                if(list.Count() == 1)
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

        // GET: Accounts/Edit/5
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

        // POST: Accounts/Edit/5
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

        // GET: Accounts/Delete/5
        public IActionResult Delete(string id)
        {
            try
            {
                _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_DeleteAccount @id = {id}");
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

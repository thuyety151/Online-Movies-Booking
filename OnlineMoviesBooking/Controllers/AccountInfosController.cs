using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModel;

namespace OnlineMoviesBooking.Controllers
{
    public class AccountInfosController : Controller
    {
        private readonly CinemaContext _context;

        public AccountInfosController(CinemaContext context)
        {
            _context = context;
        }

        // GET: AccountInfos
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Login") == null)
            {
                TempData["msg"] = "loginfirst";
                return RedirectToAction("Login", "Login");
            }
            if (HttpContext.Session.GetString("Login") != null)
            {
                string id = HttpContext.Session.GetString("Login").ToString();
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList();
                TempData["Logininf"] = account[0].Name;
                TempData["src"] = account[0].Image;
                TempData["Key"] = account[0].Id;
            }
            var acc = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{HttpContext.Session.GetString("Key")}'").ToList()[0];
            return View(acc);
        }

        // GET: AccountInfos/Edit/5
        [HttpGet]
        public IActionResult Edit(/*string Id*/)
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string id = HttpContext.Session.GetString("Login").ToString();
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList();
                TempData["Logininf"] = account[0].Name;
                TempData["src"] = account[0].Image;
            }
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList()[0];
            //AccountViewModel accountViewModel = new AccountViewModel()
            //{
            //    Id = account.Id,
            //    Name = account.Name,

            //};
            //if (account == null)
            //{
            //    return NotFound();
            //}
            //ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
            return View(/*accountViewModel*/);
        }

        // POST: AccountInfos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Birthdate,Gender,Address,Sdt,Email,Password,Point,IdTypesOfUser,IdTypeOfMember,Image")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
            return View(account);
        }

        
    }
}

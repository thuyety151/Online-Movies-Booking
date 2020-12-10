using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModel;
namespace OnlineMoviesBooking.Controllers
{
    public class ContactViewController : Controller
    {
        private readonly CinemaContext _context;

        public ContactViewController(CinemaContext context)
        {
            _context = context;
        }
        public IActionResult AllQuestion()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string id = HttpContext.Session.GetString("Login").ToString();
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList();
                TempData["Logininf"] = account[0].Name;
                TempData["src"] = account[0].Image;
            }
            var list = _context.Qa.FromSqlRaw("EXEC dbo.USP_GetAllQa").ToList();
            return View(list);
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                string id = HttpContext.Session.GetString("Login").ToString();
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList();
                TempData["Logininf"] = account[0].Name;
                TempData["src"] = account[0].Image;
            }
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContactView([Bind("Email,Content")] ContactViewModel contactView)
        {
            if (ModelState.IsValid)
            {
                Qa qa = new Qa()
                {
                    Email = contactView.Email,
                    Content = contactView.Content,
                };
                qa.Id = Guid.NewGuid().ToString();
                qa.Time = DateTime.Now;
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetAccountForEmail @Email = '{contactView.Email}'").ToList();
                qa.IdAccount = account[0].Id;
                _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertQa @id = {qa.Id}, @idaccount = {qa.IdAccount},@email = {qa.Email}, @time = {qa.Time}, @content ={qa.Content} ");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToActionPermanent("Index", contactView);
        }
    }
}

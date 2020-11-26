using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Controllers
{
    
    public class LoginController : Controller
    {
        CinemaContext _signManager = new CinemaContext();
        private readonly CinemaContext _context;

        public LoginController(CinemaContext context)
        {
            _context = context;
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Account account)
        {
          

          
            var data = _signManager.Account.Where(s => s.Email.Equals(account.Email) && s.Password.Equals(account.Password)).ToList();
            if (data.Count() != 0)
                return RedirectToAction("Dashboard", "Dashboard");
            else
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
                return View(); 
            }

        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        //public async Task<IActionResult> LoginUser(string Email, string password)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var count = _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_getIdAccount @Email = '{Email}',@pass = '{password}'");
        //            if(count == 1)
        //            {
        //                return ()
        //            }    
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Account account)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            account.Id = Guid.NewGuid().ToString();

        //            _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertUpdateAccount @id = {account.Id},@name = {account.Name},@Email={account.Email},@password={account.Password},@usertypeid={2},@action ={"Insert"} ");
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    catch
        //    {
        //        var modelstateKey = ModelState.Keys;
        //        var list = _context.Account.FromSqlRaw($"EXEC dbo.USP_CheckEmail '{account.Email}' ").ToList();
        //        if (list.Count() == 1)
        //        {
        //            ModelState.AddModelError("Email", "Email đã tồn tại");
        //        }
        //        //else if (check_db == 1)
        //        //{
        //        //    ModelState.AddModelError("Sdt", "Sdt đã tồn tại");
        //        //}
        //        return View();
        //    }

    }
    
}

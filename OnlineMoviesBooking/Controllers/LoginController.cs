using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.ViewModel;
using OnlineMoviesBooking.Models.Models;
using System.Net;
using System.Net.Mail;
using System.Configuration;
namespace OnlineMoviesBooking.Controllers
{

    public class LoginController : Controller
    {
        private readonly CinemaContext _signManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        public LoginController(CinemaContext context, IWebHostEnvironment hostEnvironment)
        {
            _signManager = context;
            this._hostEnvironment = hostEnvironment;
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            
            var acc = _signManager.Account.FromSqlRaw($"EXEC dbo.USP_GetAccountForEmail @Email = '{Email}'").ToList();
            if (acc.Count() == 1)
            {
                //đặt lại mật khẩu
                string newpass = Guid.NewGuid().ToString().Substring(26);

                _signManager.Database.ExecuteSqlCommand($"EXEC dbo.USP_ResetPassword {acc[0].Email}, {newpass} ");

                //Gửi mật khẩu qua mail
                MailMessage mm = new MailMessage("thanhtontran115@gmail.com","silentloveinheart@gmail.com");
                mm.Subject = "new password";
                mm.Body = string.Format("Hello : <h1>{0}<h1> is your password", newpass);
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential();
                nc.UserName = "thanhtontran115@gmail.com";
                nc.Password = "1152000toan";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Port = 587;
                smtp.Send(mm);
                return RedirectToAction("Login");
            }
            else
            {
                TempData["msg"] = "Gmail không tồn tại";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Account account)
        {
            var data = _signManager.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{account.Id}'").ToList();
            var ID_USER = data[0].Id;
            if( ID_USER != "")
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }       
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
        public async Task<IActionResult> Create([Bind("Email", "Password","PasswordConfirm")] RegisterViewModel registerViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Account account = new Account()
                    {
                        Email = registerViewModel.Email,
                        Password = registerViewModel.Password,
                    };
                    account.Id = Guid.NewGuid().ToString();

                    _signManager.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertUpdateAccount @id = {account.Id},@name = {account.Name},@birthdate = {account.Birthdate},@gender={account.Gender},@address={account.Address},@SDT={account.Sdt},@Email={account.Email},@password={account.Password},@point ={account.Point},@usertypeid={account.IdTypesOfUser},@membertypeid = {account.IdTypeOfMember}, @image={account.Image},@action ={"Insert"} ");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                var modelstateKey = ModelState.Keys;
                var list = _signManager.Account.FromSqlRaw($"EXEC dbo.USP_CheckEmail '{registerViewModel.Email}' ").ToList();
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
            return View(registerViewModel);
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

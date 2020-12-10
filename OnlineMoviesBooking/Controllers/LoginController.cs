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
using Microsoft.AspNetCore.Session;
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
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData.Clear();
            return RedirectToAction("Index","Home");
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
                MailMessage mm = new MailMessage("thanhtontran115@gmail.com","quynhkun27@gmail.com");
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

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Username", "FullName", "Email", "SDT", "Password", "PasswordConfirm")] RegisterViewModel registerViewModel)
        {
            //3 mail trùng
            //1 login trùng
            //2 user trùng
            try
            {
                if (ModelState.IsValid)
                {
                    var acc = _signManager.Account.FromSqlRaw($"EXEC dbo.USP_GetAccountForEmail @Email = '{registerViewModel.Email}' ").ToList();
                    if (acc.Count == 1)
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại");
                        return View(registerViewModel);
                    }

                    Account account = new Account()
                    {
                        Email = registerViewModel.Email,
                        Password = registerViewModel.Password,
                    };
                    account.Name = registerViewModel.FullName;
                    account.Sdt = registerViewModel.SDT;
                    account.Id = registerViewModel.Username;
                    account.IdTypeOfMember = "mobile";
                    _signManager.Database.ExecuteSqlCommand($"EXEC [dbo].[USP_CreateUser] {account.Id},{account.Id},{account.Password},{"CUSTOMER"}");

                    _signManager.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertUpdateAccount @id = {account.Id},@name = {account.Name},@birthdate = {"11-5-2000"},@gender={account.Gender},@address={account.Address},@SDT={account.Sdt},@Email={account.Email},@password={account.Password},@point ={account.Point},@usertypeid={"2"},@membertypeid = {account.IdTypeOfMember}, @image={account.Image},@action ={"Insert"} ");
                    HttpContext.Session.GetString(registerViewModel.Username);
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (SqlException e)
            {
                if (e.ToString().Contains("User"))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                }


                return View(registerViewModel);
            }
            return View(registerViewModel);

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username", "Password")] LoginModelView loginModelView)
        {
            try
            {
                
                var acc = _signManager.Account.FromSqlRaw($"EXEC dbo.USP_CheckForLogin @Name = '"+loginModelView.Username+ 
                     "' , @Password = '"+loginModelView.Password + "'").ToList();
               

                if ( acc.Count() != 1)
                {
                    ViewData["Error"] = "Ten dang nhap hoac mat khau khong đung";
                    return View(loginModelView);
                }
                
                HttpContext.Session.SetString("Key", acc[0].Id);

                if (acc[0].IdTypesOfUser == "1")
                {
                    HttpContext.Session.SetString("Login", acc[0].Id);
                    string id = HttpContext.Session.GetString("Login").ToString();
                    var account = _signManager.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList();
                    TempData["Logininf"] = account[0].Name;
                    TempData["src"] = account[0].Image;
                    return RedirectToAction("Index", "Accounts");
                }
                else if (acc[0].IdTypesOfUser == "2")
                {

                    HttpContext.Session.SetString("Login", acc[0].Id);
                    string id = HttpContext.Session.GetString("Login").ToString();
                    var account = _signManager.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList();
                    TempData["Logininf"] = account[0].Name;
                    TempData["src"] = account[0].Image;

                    return RedirectToAction("Index", "Home");
                }

                ViewData["Error"] = "Something wrong!!!";
                return View(loginModelView);
            }
            catch (SqlException e)
            {
                ModelState.AddModelError("", e.ToString());
                return View(loginModelView);
            }
        }
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

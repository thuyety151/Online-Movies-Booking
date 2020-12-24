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
using OnlineMoviesBooking.DataAccess.Data;
namespace OnlineMoviesBooking.Controllers
{

    public class LoginController : Controller
    {

        private readonly IWebHostEnvironment _hostEnvironment;

        public LoginController(IWebHostEnvironment hostEnvironment)
        {

            this._hostEnvironment = hostEnvironment;
        }
        public ActionResult ForgotPassword()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            return View();
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {

            string connectionString = "Server=localhost;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

            //đặt lại mật khẩu
            string newpass = Guid.NewGuid().ToString().Substring(26);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = $"EXEC dbo.USP_ResetPassword @email = '{Email}',@pw = '{newpass}'";
                string conmandText2 = $"EXEC dbo.USP_GetAccountForEmail @Email = '{Email}'";
                string loginname = "";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    if (command.ExecuteNonQuery() > 0)
                    {
                        //Gửi mật khẩu qua mail
                        MailMessage mm = new MailMessage("thanhtontran115@gmail.com", "silentloveinheart@gmail.com");
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
                    }
                }
                catch (SqlException e)
                {
                    TempData["msg"] = "error";
                    return View();
                }
                command = new SqlCommand(conmandText2, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        loginname = Convert.ToString(reader[0]);
                    }
                }
                catch (SqlException e)
                {
                    TempData["msg"] = "error";
                    return View();
                }
                string conmandText3 = $"ALTER LOGIN {loginname} WITH PASSWORD = '{newpass}'";
                command = new SqlCommand(conmandText3, connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    TempData["msg"] = "error";
                    return View();
                }
                connection.Close();
            }


            return RedirectToAction("Login");


        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register([Bind("Username", "FullName", "Email", "SDT", "Password", "PasswordConfirm")] RegisterViewModel registerViewModel)
        {
            //3 mail trùng
            //1 login trùng
            //2 user trùng
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = "Server=localhost;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string commandText = $"EXEC dbo.USP_InsertUpdateAccount @id = '{registerViewModel.Username}',@name = N'{registerViewModel.FullName}',@birthdate = {DateTime.Now.Day},"
                        + $"@gender = {1},@address = '{""}',@SDT = '{registerViewModel.SDT}',@Email = '{registerViewModel.Email}',"
                        + $"@password = '{registerViewModel.Password}',@point = {0},@usertypeid = '{2}',@membertypeid = 'mobile',"
                        + $"@image = '{""}',@action = 'Insert'";

                        var command = new SqlCommand(commandText, connection);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            if (e.ToString().Contains("User") || e.ToString().Contains("Email"))
                                ModelState.AddModelError("", @"User hoặc Email đã tồn tại!!");
                            return View(registerViewModel);
                        }
                        connection.Close();
                    }
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
            if (HttpContext.Session.GetString("idLogin") == null)
                return View();
            else
            {
                TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
                TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
                TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
                TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Login([Bind("Username", "Password")] LoginModelView loginModelView)
        {
            try
            {
                
                Account acc = new Account();
                List<TypeOfMember> listmember = new List<TypeOfMember>();
                List<TypesOfAccount> listaccount = new List<TypesOfAccount>();
                string connectionString = "Server=localhost;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_CheckForLogin @username = '{loginModelView.Username}' , @password = '{loginModelView.Password}'";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                acc.Id = Convert.ToString(reader[0]);
                                acc.Name = Convert.ToString(reader[1]);
                                acc.Birthdate = Convert.ToDateTime(reader[2]);
                                acc.Gender = Convert.ToBoolean(reader[3]);
                                acc.Address = Convert.ToString(reader[4]);
                                acc.Sdt = Convert.ToString(reader[5]);
                                acc.Email = Convert.ToString(reader[6]);
                                acc.Password = Convert.ToString(reader[7]);
                                acc.Point = Convert.ToInt32(reader[8]);
                                acc.IdTypesOfUser = Convert.ToString(reader[9]);
                                acc.IdTypeOfMember = Convert.ToString(reader[10]);
                                acc.Image = Convert.ToString(reader[11]);
                            }

                        }
                        else
                        {
                            TempData["msg"] = "wrong";
                            return View(loginModelView);
                        }
                    }
                    catch (SqlException e)
                    {

                        ModelState.AddModelError("", e.ToString());
                        return View(loginModelView);
                    }
                    connection.Close();

                }

                HttpContext.Session.SetString("idLogin", acc.Id);
                HttpContext.Session.SetString("nameLogin", acc.Name);
                HttpContext.Session.SetString("imgLogin", acc.Image);
                HttpContext.Session.SetString("pwLogin", acc.Password);
                HttpContext.Session.SetString("roleLogin", acc.IdTypesOfUser);
                HttpContext.Session.SetString("connectString", $"Server=localhost;Database=Cinema;MultipleActiveResultSets=true;User Id={acc.Id};Password={acc.Password}");
                TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
                TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
                TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
                TempData["pwLogin"] = HttpContext.Session.GetString("pwLogin");
                TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
                return RedirectToAction("Index", "Home");
            }
            catch (SqlException e)
            {
                ModelState.AddModelError("", e.ToString());
                return View(loginModelView);
            }
        }


    }

}

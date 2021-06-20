using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModel;

namespace OnlineMoviesBooking.Controllers
{

    public class AccountInfosController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public AccountInfosController(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
        }
       
        
        public IActionResult Index()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            if (HttpContext.Session.GetString("idLogin") == null)
            {
                TempData["msg"] = "Dang nhap truoc";
                return RedirectToAction("Login", "Login");
            }
            
            Account acc = new Account();
            string connectionString = HttpContext.Session.GetString("connectString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = $"EXEC dbo.USP_GetDetailAccount @id = '{HttpContext.Session.GetString("idLogin")}'";

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
                        TempData["msg"] = "error";
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch
                {

                    TempData["msg"] = "error";
                    return RedirectToAction("Index", "Home");
                }
                connection.Close();
            }

            return View(acc);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            if (HttpContext.Session.GetString("idLogin") == null)
            {
                TempData["msg"] = "Dang nhap truoc";
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword([Bind("OldPasswword,NewPassword,ComfirmNewPassword")] ChangePasswordViewModel changePassword)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            if (HttpContext.Session.GetString("idLogin") == null)
            {
                TempData["msg"] = "Dang nhap truoc";
                return RedirectToAction("Login", "Login");
            }
            string connectionString = HttpContext.Session.GetString("connectString");
            string username = HttpContext.Session.GetString("idLogin");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = $"EXEC dbo.USP_ChangePassword @username = '{username}',@OldPw = '{changePassword.OldPasswword}',@NewPw = '{changePassword.NewPassword}',@ConfirmNewPw = '{changePassword.ComfirmNewPassword}'";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.RecordsAffected  != 1)
                    {
                        TempData["msgchange"] = @"Mật Khẩu không đúng!!";
                        return View(changePassword);
                    }
                    
                }
                catch (SqlException e)
                {

                    TempData["msg"] = "Dang nhap truoc";
                    ModelState.AddModelError("", e.ToString());
                    return RedirectToAction("Index", "Home");
                }
                connection.Close();
                HttpContext.Session.SetString("connectString", $"Server=localhost\\SQLEXPRESS;Database=Cinema;MultipleActiveResultSets=true;User Id={username};Password={changePassword.NewPassword}");
                HttpContext.Session.SetString("pwLogin", changePassword.NewPassword);
            }

            return RedirectToAction("Index","AccountInfos");
        }

        // GET: AccountInfos/Edit/5
        [HttpGet]
       // [ValidateAntiForgeryToken]
        public IActionResult Edit()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            if (HttpContext.Session.GetString("idLogin") == null)
            {
                TempData["msg"] = "loginfirst";
                return RedirectToAction("Login", "Login");
            }
            
            AccountViewModel acc = new AccountViewModel();

            string connectionString = HttpContext.Session.GetString("connectString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = $"EXEC dbo.USP_GetDetailAccount @id = '{HttpContext.Session.GetString("idLogin")}'";

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

                        }

                    }
                    else
                    {
                        TempData["msg"] = "error";
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch
                {

                    TempData["msg"] = "error";
                    return RedirectToAction("Index", "Home");
                }
                connection.Close();
            }

            return View(acc);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,Birthdate,Gender,Address,Sdt,Email,Password,Point,IdTypesOfUser,IdTypeOfMember,Image")] AccountViewModel account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }
            
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string img = "";
                if (account.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"image\Account\");
                    var extension = Path.GetExtension(account.Image.FileName);

                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        account.Image.CopyTo(filesStreams);
                    }
                    img = @"\image\Account\" + fileName + extension;
                }
                else
                {
                    img = HttpContext.Session.GetString("imgLogin");
                }
                try
                {

                    string connectionString = HttpContext.Session.GetString("connectString");

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string commandText = $"EXEC dbo.USP_InsertUpdateAccount @id = '{account.Id}',@name = N'{account.Name}',@birthdate = '{account.Birthdate}',"
                        + $"@gender = {account.Gender},@address = '{account.Address}',@SDT = '{account.Sdt}',@Email = '{account.Email}',"
                        + $"@password = '{account.Password}',@point = {account.Point},@usertypeid = '{account.IdTypesOfUser}',@membertypeid = 'mobile',"
                        + $"@image = '{img}',@action = 'Update'";

                        var command = new SqlCommand(commandText, connection);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            if (e.ToString().Contains("User") || e.ToString().Contains("Email"))
                                ModelState.AddModelError("", @"User hoặc Email đã tồn tại!!");
                            return View(account);
                        }
                        connection.Close();
                    }
                    HttpContext.Session.SetString("nameLogin", account.Name);

                    HttpContext.Session.SetString("imgLogin", img);
                    return RedirectToAction("Index", "AccountInfos");

                }
                catch (SqlException e)
                {
                    if (e.ToString().Contains("User"))
                    {
                        ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                    }


                    return View(account);
                }

            }
            //ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
            return RedirectToAction("Index", "AccountInfos");
        }

        public IActionResult GetAllTicket()
        {
           
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            string username = HttpContext.Session.GetString("idLogin").ToString();
            List<Ticket> listTicket = new List<Ticket>();
            string connectionString = HttpContext.Session.GetString("connectString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = $"EXEC dbo.USP_GetAllTicketOfAccount @username = '{username}'";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Ticket ticket = new Ticket();
                        ticket.Id = Convert.ToString(reader[0]);
                        ticket.Date = Convert.ToDateTime(reader[1]);
                        ticket.Point = Convert.ToInt32(reader[2]);
                        //ticket.Status = Convert.ToBoolean(reader[3]);
                        ticket.IdShow = Convert.ToString(reader[4]);
                        ticket.IdAccount = Convert.ToString(reader[5]);
                        ticket.IdDiscount = Convert.ToString(reader[6]);
                        ticket.IdSeat = Convert.ToString(reader[7]);
                        listTicket.Add(ticket);
                    }

                }
                catch (SqlException e)
                {

                    TempData["msg"] = e.ToString();
                    return RedirectToAction("Index", "Home");
                }
                connection.Close();
            }

            return View(listTicket);
        }
    }
}

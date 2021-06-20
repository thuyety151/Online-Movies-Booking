using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModel;
using System.Web;

namespace OnlineMoviesBooking.Areas.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string check;
        public AccountsController(IWebHostEnvironment hostEnvironment,IHttpContextAccessor httpContextAccessor)
        {
            this._hostEnvironment = hostEnvironment;
            string username = httpContextAccessor.HttpContext.Session.GetString("idLogin");
            string connectionString = httpContextAccessor.HttpContext.Session.GetString("connectString");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = $"EXEC dbo.USP_CheckAdmin @username = '{username}' ";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        check = Convert.ToString(reader[0]);
                    }    
                }
                catch (SqlException e)
                {
                    connection.Close();
                    check = "0";
                }
                connection.Close();
            }

        }

        // GET: Accounts
        public IActionResult Index()
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null )
            {
                if(check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }    
               
            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }    

            //return View(listacc);
            return View();
        }

        public IActionResult getall()
        {
            List<Account> listacc = new List<Account>();
            string connectionString = HttpContext.Session.GetString("connectString");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = "EXEC dbo.USP_SelectAllAccount";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Account acc = new Account();
                        acc.Id = Convert.ToString(reader[0]);
                        acc.Name = Convert.ToString(reader[1]);
                        acc.Birthdate = Convert.ToDateTime(reader[2]);
                        acc.Gender = Convert.ToBoolean(reader[3]);
                        acc.Address = HttpUtility.HtmlDecode(Convert.ToString(reader[4]));
                        acc.Sdt = Convert.ToString(reader[5]);
                        acc.Email = Convert.ToString(reader[6]);
                        acc.Password = HttpUtility.HtmlDecode(Convert.ToString(reader[7]));
                        acc.Point = Convert.ToInt32(reader[8]);
                        acc.IdTypesOfUser = Convert.ToString(reader[9]);
                        acc.IdTypeOfMember = Convert.ToString(reader[10]);
                        acc.Image = Convert.ToString(reader[11]);
                        listacc.Add(acc);
                    }
                }
                catch (SqlException e)
                {
                    connection.Close();
                    return Json(new { data = e.Message });
                }
                connection.Close();

            }
            return Json(new { data = listacc });
        }
        public IActionResult Details(string id)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            try
            {

                Account acc = new Account();
                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_GetDetailAccount @id = '{id}'";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                acc.Id = Convert.ToString(reader[0]);
                                acc.Name = HttpUtility.HtmlDecode(Convert.ToString(reader[1]));
                                acc.Birthdate = Convert.ToDateTime(reader[2]);
                                acc.Gender = Convert.ToBoolean(reader[3]);
                                acc.Address = HttpUtility.HtmlDecode(Convert.ToString(reader[4]));
                                acc.Sdt = Convert.ToString(reader[5]);
                                acc.Email = Convert.ToString(reader[6]);
                                acc.Password = HttpUtility.HtmlDecode(Convert.ToString(reader[7]));
                                acc.Point = Convert.ToInt32(reader[8]);
                                acc.IdTypesOfUser = Convert.ToString(reader[9]);
                                acc.IdTypeOfMember = Convert.ToString(reader[10]);
                                acc.Image = Convert.ToString(reader[11]);
                            }
                        }
                        else
                        {
                            TempData["msg"] = "error";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        return RedirectToAction(nameof(Index));
                    }
                    connection.Close();
                }   
                return View(acc);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        // GET: Accounts/Create
        public IActionResult Create()
        {

            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }

            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id", "Name", "Birthdate", "Gender", "Address", "Sdt", "Email", "Password", "Point", "IdTypesOfUser", "IdTypeOfMember", "Image")] Account account, IFormFile files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    account.Name =HttpUtility.HtmlEncode(account.Name);
                    account.Address = HttpUtility.HtmlEncode(account.Address);
                    // save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    if (files != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"image\Account\");
                        var extension = Path.GetExtension(files.FileName);

                        if (account.Image != null)
                        {
                            // edit 
                            var imagePath = Path.Combine(wwwRootPath, account.Image.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }

                        }
                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            files.CopyTo(filesStreams);
                        }
                        account.Image = @"\image\Account\" + fileName + extension;
                    }

                    try
                    {
                        if (ModelState.IsValid)
                        {
                            string connectionString = HttpContext.Session.GetString("connectString");

                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                string commandText = $"EXEC dbo.USP_InsertUpdateAccount @id = '{account.Id}',@name = N'{account.Name}',@birthdate = '{account.Birthdate}',"
                                + $"@gender = {account.Gender},@address = '{account.Address}',@SDT = '{account.Sdt}',@Email = '{account.Email}',"
                                + $"@password = '{account.Password}',@point = {account.Point},@usertypeid = '{account.IdTypesOfUser}',@membertypeid = '{account.IdTypeOfMember}',"
                                + $"@image = '{account.Image}',@action = 'Insert'";

                                var command = new SqlCommand(commandText, connection);
                                try
                                {
                                    command.ExecuteNonQuery();
                                }
                                catch (SqlException e)
                                {
                                    connection.Close();
                                    if (e.ToString().Contains("User") || e.ToString().Contains("Email"))
                                        ModelState.AddModelError("", @"User hoặc Email đã tồn tại!!");
                                    return View(account);
                                }
                                connection.Close();
                            }

                            return RedirectToAction(nameof(Index));
                        }
                    }
                    catch (SqlException e)
                    {

                        if (e.ToString().Contains("User"))
                        {
                            ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                        }


                        return View(account);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }

            return View(account);
        }

        // GET: Accounts/Edit/5
        public IActionResult Edit(string id)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            if (id == null)
            {
                return NotFound();
            }
            Account acc = new Account();
            try
            {


                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_GetDetailAccount @id = '{id}'";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                acc.Id = HttpUtility.HtmlDecode(Convert.ToString(reader[0]));
                                acc.Name = HttpUtility.HtmlDecode(Convert.ToString(reader[1]));
                                acc.Birthdate = Convert.ToDateTime(reader[2]);
                                acc.Gender = Convert.ToBoolean(reader[3]);
                                acc.Address = HttpUtility.HtmlDecode(Convert.ToString(reader[4]));
                                acc.Sdt = Convert.ToString(reader[5]);
                                acc.Email = Convert.ToString(reader[6]);
                                acc.Password = HttpUtility.HtmlDecode(Convert.ToString(reader[7]));
                                acc.Point = Convert.ToInt32(reader[8]);
                                acc.IdTypesOfUser = Convert.ToString(reader[9]);
                                acc.IdTypeOfMember = Convert.ToString(reader[10]);
                                acc.Image = Convert.ToString(reader[11]);
                            }

                        }
                        else
                        {
                            TempData["msg"] = "error";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        TempData["msg"] = "error";
                        return RedirectToAction(nameof(Index));
                    }
                    connection.Close();
                }

                return View(acc);
            }
            catch (Exception e)
            {

                return RedirectToAction(nameof(Index));
            }


        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IFormFile files, [Bind("Id", "Name", "Birthdate", "Gender", "Address", "Sdt", "Email", "Password", "Point", "IdTypesOfUser", "IdTypeOfMember", "Image")] Account account)
        {

            if (id != account.Id)
            {
                return NotFound();
            }

                try
                {
                    account.Name = HttpUtility.HtmlEncode(account.Name);
                    account.Address = HttpUtility.HtmlEncode(account.Address);
                string connectionString = HttpContext.Session.GetString("connectString");

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string commandText = $"EXEC dbo.USP_InsertUpdateAccount @id = '{account.Id}',@name = N'{account.Name}',@birthdate = '{account.Birthdate}',"
                        + $"@gender = {account.Gender},@address = '{account.Address}',@SDT = '{account.Sdt}',@Email = '{account.Email}',"
                        + $"@password = '{account.Password}',@point = {account.Point},@usertypeid = '{account.IdTypesOfUser}',@membertypeid = 'mobile',"
                        + $"@image = '{account.Image}',@action = 'Update'";

                        var command = new SqlCommand(commandText, connection);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            connection.Close();
                            if (e.ToString().Contains("User") || e.ToString().Contains("Email"))
                                ModelState.AddModelError("", @"User hoặc Email đã tồn tại!!");
                            return View(account);
                        }
                        connection.Close();
                    }

                    return RedirectToAction(nameof(Index));

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
        [HttpDelete]
        // GET: Accounts/Delete/5
        public IActionResult Delete(string id)
        {
            TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                if (check == "0")
                {
                    TempData["msg"] = "Khong duoc phep truy cap";
                    return Redirect("/Home/Index");
                }

            }
            else
            {
                TempData["msg"] = "Chua dang nhap";
                return Redirect("/Home/Index");
            }
            try
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                var image = HttpContext.Session.GetString("imgLogin");
                if (image != "")
                {
                    var imageUrl = wwwRootPath + image;
                    System.IO.File.Delete(imageUrl);
                }
                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_DeleteAccount @id = '{id}' ";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        connection.Close();

                        return Json(new { success = false, message = "Tài khoản không tồn tại! Vui lòng tải lại trang" });
                    }
                    connection.Close();
                }

                return Json(new { success = true, message = "Xóa mục thành công" });

            }
            catch (SqlException e)
            {
                if (e.ToString().Contains("User"))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                }
                return Json(new { success = false, message = "Tài khoản không tồn tại! Vui lòng tải lại trang" });
            }
        }
    }
}

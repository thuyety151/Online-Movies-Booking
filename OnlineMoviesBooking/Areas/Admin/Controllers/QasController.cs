using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Areas.Controllers
{
    [Area("Admin")]
    public class QasController : Controller
    {
        private readonly string check;

        public QasController(IHttpContextAccessor httpContextAccessor)
        {
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
                    while (reader.Read())
                    {
                        check = Convert.ToString(reader[0]);
                    }
                }
                catch (SqlException e)
                {
                    connection.Close();
                    check = "0";
                }
                check = "1";
                connection.Close();
            }
        }

        // GET: Qas
        public IActionResult Index()
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
            List<Qa> listqa = new List<Qa>();
            string connectionString = HttpContext.Session.GetString("connectString");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = "EXEC dbo.USP_GetAllQa";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Qa qa = new Qa();
                            qa.Id = Convert.ToString(reader[0]);
                            qa.IdAccount = Convert.ToString(reader[1]);
                            qa.Email = Convert.ToString(reader[2]);
                            qa.Time = Convert.ToDateTime(reader[3]);
                            qa.Content = Convert.ToString(reader[4]);
                            listqa.Add(qa);
                        }

                    }
                    else
                    {
                        TempData["msg"] = "error";
                        return RedirectToAction("HomeAdmin", "HomeAdmin");
                    }
                }
                catch (SqlException e)
                {
                    connection.Close();
                    return RedirectToAction("HomeAdmin", "HomeAdmin");
                }
                connection.Close();

            }

            return View(listqa);
        }


        public IActionResult Get(string id)
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

                Qa qa = new Qa();
                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_GetQa @id = '{id}'";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                qa.Id = Convert.ToString(reader[0]);
                                qa.IdAccount = Convert.ToString(reader[1]);
                                qa.Email = Convert.ToString(reader[2]);
                                qa.Time = Convert.ToDateTime(reader[3]);
                                qa.Content = Convert.ToString(reader[4]);

                            }

                        }
                        else
                        {
                            TempData["msg"] = "error";
                            return Json(new { success = false, message = "Lỗi!" });
                        }
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        TempData["msg"] = "error";
                        return Json(new { success = false, message = e.Message });
                    }
                    connection.Close();

                }

                return Json(new { data = qa });
            }
            catch (Exception e)
            {

                return Json(new { data = e.Message });


            }

        }

        //// GET: Qas/Create
        //public IActionResult Create()
        //{
        //    TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
        //    TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
        //    TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
        //    if (HttpContext.Session.GetString("idLogin") == null || HttpContext.Session.GetString("roleLogin") != "1")
        //    {

        //        TempData["msg"] = "Error";
        //        return Redirect("/Home/Index");
        //    }
        //    //ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id");
        //    return View();
        //}

        //// POST: Qas/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("Email,Content")] Qa qa)
        //{
        //    TempData["idlogin"] = HttpContext.Session.GetString("idLogin");
        //    TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
        //    TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
        //    if (ModelState.IsValid)
        //    {
        //        qa.Id = Guid.NewGuid().ToString();
        //        qa.Time = DateTime.Now;
        //        try
        //        {


        //            string connectionString = HttpContext.Session.GetString("connectString");

        //            using (var connection = new SqlConnection(connectionString))
        //            {
        //                connection.Open();
        //                string commandText = $"EXEC dbo.USP_InsertQa @id = '{qa.Id}',@email = '{qa.Email}', @time = '{qa.Time}', @content = N'{qa.Content}' ";

        //                var command = new SqlCommand(commandText, connection);
        //                try
        //                {
        //                    command.ExecuteNonQuery();
        //                }
        //                catch (SqlException e)
        //                {
        //                    connection.Close();
        //                    TempData["msg"] = "error";
        //                    return RedirectToAction(nameof(Index));
        //                }
        //                connection.Close();
        //            }

        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (Exception e)
        //        {

        //            return View(qa);
        //        }
        //    }
        //    return View(qa);
        //}

        //// GET: Qas/Edit/5
        //public IActionResult Edit(string id)
        //{
        //    TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
        //    TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
        //    TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
        //    if (HttpContext.Session.GetString("idLogin") == null || HttpContext.Session.GetString("roleLogin") != "1")
        //    {

        //        TempData["msg"] = "Error";
        //        return Redirect("/Home/Index");
        //    }
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Qa qa = new Qa();
        //    try
        //    {


        //        string connectionString = HttpContext.Session.GetString("connectString");

        //        using (var connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            string commandText = $"EXEC dbo.USP_GetQa @id = '{id}'";

        //            var command = new SqlCommand(commandText, connection);
        //            try
        //            {
        //                SqlDataReader reader = command.ExecuteReader();

        //                if (reader.HasRows)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        qa.Id = Convert.ToString(reader[0]);
        //                        qa.IdAccount = Convert.ToString(reader[1]);
        //                        qa.Email = Convert.ToString(reader[2]);
        //                        qa.Time = Convert.ToDateTime(reader[3]);
        //                        qa.Content = Convert.ToString(reader[4]);

        //                    }

        //                }
        //                else
        //                {
        //                    TempData["msg"] = "error";
        //                    return RedirectToAction(nameof(Index));
        //                }
        //            }
        //            catch (SqlException e)
        //            {
        //                connection.Close();
        //                TempData["msg"] = "error";
        //                return RedirectToAction(nameof(Index));
        //            }
        //            connection.Close();
        //        }

        //        return View(qa);
        //    }
        //    catch (Exception e)
        //    {

        //        return View(qa);
        //    }


        //}

        //// POST: Qas/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(string id, [Bind("Id,IdAccount,Email,Time,Content")] Qa qa)
        //{

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    TypeOfMember member = new TypeOfMember();
        //    try
        //    {


        //        string connectionString = HttpContext.Session.GetString("connectString");

        //        using (var connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            string commandText = $"EXEC dbo.USP_UpdateQA @id = '{qa.Id}', @time = '{qa.Time}', @content = N'{qa.Content}'";

        //            var command = new SqlCommand(commandText, connection);
        //            try
        //            {
        //                command.ExecuteNonQuery();
        //            }
        //            catch (SqlException e)
        //            {
        //                connection.Close();
        //                TempData["msg"] = "error";
        //                return RedirectToAction(nameof(Index));
        //            }
        //            connection.Close();
        //        }

        //        return View(member);
        //    }
        //    catch (Exception e)
        //    {

        //        return View(qa);
        //    }

        //}

        // POST: Qas/Delete/5
        [HttpDelete]
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

                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_DeleteQuestionAnswer @id = '{id}'";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        return Json(new { success = false, mesage = "Xóa không thành công" });
                    }
                    connection.Close();
                }

                return Json(new { success = true, message = "Xóa mục thành công" });

            }
            catch (SqlException e)
            {
                return Json(new { success = false, mesage = "Xóa không thành công" });
            }

        }


    }
}

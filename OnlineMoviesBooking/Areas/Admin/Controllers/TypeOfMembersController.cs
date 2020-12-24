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
    public class TypeOfMembersController : Controller
    {
        private readonly string check;

        public TypeOfMembersController(IHttpContextAccessor httpContextAccessor)
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
                connection.Close();
            }
        }

        // GET: TypeOfMembers
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
            List<TypeOfMember> listmember = new List<TypeOfMember>();
            try
            {


                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_GetAllMemberType";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                TypeOfMember member = new TypeOfMember();
                                member.IdTypeMember = Convert.ToString(reader[0]);
                                member.TypeOfMemberName = Convert.ToString(reader[1]);
                                member.Content = Convert.ToString(reader[2]);
                                member.Point = Convert.ToInt32(reader[3]);
                                member.Money = Convert.ToDouble(reader[4]);
                                listmember.Add(member);
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

                return View(listmember);
            }
            catch (Exception e)
            {

                return RedirectToAction(nameof(Index));
            }


        }



        // GET: TypeOfMembers/Create
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

        // POST: TypeOfMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TypeOfMember typeOfMember)
        {
            if (ModelState.IsValid)
            {
                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_InsertMemberType @id = '{typeOfMember.IdTypeMember}',  @Name = '{typeOfMember.TypeOfMemberName}', @content = N'{typeOfMember.Content}',@money={typeOfMember.Money}, @point = {typeOfMember.Point}";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        return RedirectToAction(nameof(Index));
                    }
                    connection.Close();
                }

                return RedirectToAction(nameof(Index));

            }
            return View(typeOfMember);
        }

        // GET: TypeOfMembers/Edit/5
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
            TypeOfMember member = new TypeOfMember();
            try
            {


                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_GetMemberType @id = '{id}'";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                member.IdTypeMember = Convert.ToString(reader[0]);
                                member.TypeOfMemberName = Convert.ToString(reader[1]);
                                member.Content = Convert.ToString(reader[2]);
                                member.Point = Convert.ToInt32(reader[3]);
                                member.Money = Convert.ToDouble(reader[4]);

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

                return View(member);
            }
            catch (Exception e)
            {

                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        // POST: TypeOfMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(string id, TypeOfMember typeOfMember)
        {
            if (id != typeOfMember.IdTypeMember)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string connectionString = HttpContext.Session.GetString("connectString");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_UpdateMemberType @id = '{typeOfMember.IdTypeMember}',  @Name = '{typeOfMember.TypeOfMemberName}', @content = '{typeOfMember.Content}',@money = {typeOfMember.Money}, @point = {typeOfMember.Point}";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        return RedirectToAction(nameof(Index));
                    }
                    connection.Close();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(typeOfMember);
        }
        // POST: TypeOfMembers/Delete/5
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
                    string commandText = $"EXEC dbo.USP_DeleteMemberType @id = '{id}'";

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

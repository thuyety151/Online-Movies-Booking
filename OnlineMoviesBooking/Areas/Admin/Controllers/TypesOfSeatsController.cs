using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TypesOfSeatsController : Controller
    {
        private ExecuteProcedure Exec;
        private readonly string check;

        public TypesOfSeatsController(IHttpContextAccessor httpContextAccessor)
        {
            Exec = new ExecuteProcedure(httpContextAccessor.HttpContext.Session.GetString("connectString").ToString());
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


        // GET: TypesOfSeats
        public IActionResult Index()
        {
            var types = Exec.GetAllTypesOfSeat();
            return View(types);
        }

        // GET: TypesOfSeats/Details/5
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
            if (id == null)
            {
                return NotFound();
            }

            var typesOfSeat = Exec.ExecGetDetailTypeOfSeat(id);
            if (typesOfSeat == null)
            {
                return NotFound();
            }

            return View(typesOfSeat);
        }


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

            var typesOfSeat = Exec.ExecGetDetailTypeOfSeat(id);
            if (typesOfSeat == null)
            {
                return NotFound();
            }
            return View(typesOfSeat);
        }

       
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Name,Cost")] TypesOfSeat typesOfSeat)
        {
            if (id != typesOfSeat.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                string result=Exec.ExecuteUpdateTypesOfSeat(typesOfSeat);
                if (result != "")
                {
                    ModelState.AddModelError("Name", result);
                    return View(typesOfSeat);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(typesOfSeat);
        }
        
       
    }
}

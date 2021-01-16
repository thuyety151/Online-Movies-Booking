using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OnlineMoviesBooking.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BillsController : Controller
    {
        private readonly ExecuteProcedure Exec;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string check;
        public BillsController(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this._hostEnvironment = hostEnvironment;
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
                connection.Close();
            }
        }
        public IActionResult GetAll()
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
            var bill = Exec.ExecuteGetAllBillAdmin().Select(x => new
            {
                id = x.Id,
                nameAccount = x.AccountName,
                movieName = x.MovieName,
                screenName = x.ScreenName,
                theaterName = x.TheaterName,
                timeStart = x.TimeStart.ToString(),
                row = x.Row,
                no = x.No,
            });
            return Json(new { data = bill });
        }
        // GET: BillsController
        public ActionResult Index()
        {
            return View();
        }
        // GET: BillsController/Details/5
        public ActionResult Details(string id)
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
            try
            {
                var bill = Exec.ExecGetBillAdminDetail(id);
                return View(bill);
            }
            catch
            {
                return NotFound();
            }
        }
        // GET: BillsController/Delete/5
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
            Exec.ExecuteDeleteTicket(id);
            return Json(new { success = true,mesage = "xóa thành công"  });
        }
    }
}

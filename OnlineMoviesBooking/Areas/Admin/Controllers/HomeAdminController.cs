using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using OnlineMoviesBooking.Models;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Areas.Controllers
{
   
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly ILogger<HomeAdminController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string check;
        public HomeAdminController(ILogger<HomeAdminController> logger, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            
            _logger = logger;
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
        public IActionResult Index()
        {
            //TempData["idLogin"] = HttpContext.Session.GetString("idLogin");
            //TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            //TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            //if (HttpContext.Session.GetString("idLogin") != null)
            //{
            //    if (check == "0")
            //    {
            //        TempData["msg"] = "Khong duoc phep truy cap";
            //        return Redirect("/Home/Index");
            //    }

            //}
            //else
            //{
            //    TempData["msg"] = "Chua dang nhap";
            //    return Redirect("/Home/Index");
            //}
            string connectionString = "Server=localhost;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = "";

                var command = new SqlCommand(commandText, connection);
                commandText = "EXECUTE dbo.Stastistic_account";
                command = new SqlCommand(commandText, connection);
                ViewBag.Account = command.ExecuteScalar().ToString();
                
                commandText = "EXECUTE dbo.Stastistic_Count";
                command = new SqlCommand(commandText, connection);
                ViewBag.Count = command.ExecuteScalar().ToString();

                commandText = "EXECUTE dbo.Stastistic_ticket";
                command = new SqlCommand(commandText, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ViewBag.Ticket = Convert.ToString(reader[0]);
                    ViewBag.Book = Convert.ToString(reader[1]);
                }
                connection.Close();

            }
            return View();
        }
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetData()//xong qua đây  // id truyền vào null kìa// null nữa gòi
        {

            string connectionString = "Server=localhost;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";
            var label = new string[12];
            var value = new double[12];
            //var vl = cinemaContext.B
            for(int i = 0;i<12;i++)
            {
                label[i] = (i + 1).ToString();
                value[i] = 0;
            }
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string commandText = $"EXEC dbo.USP_Chart @year = 2020";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        value[Convert.ToInt32(reader[0]) - 1] = Convert.ToDouble(reader[1]);
                    }
                }
                catch
                {

                }

            }
            
            return Json(new { label, value });
            //var totalProduct = _db.OrderDetails.Include(x => x.Product)
            //    .Where(x => x.Product.ShopId == Id && x.Status == OrderDetailStatus.deliveried.ToString()).Sum(x => x.Price).ToString();
            //return NotFound();
        }
        
    }
}

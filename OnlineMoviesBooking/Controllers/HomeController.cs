using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ExecuteProcedure Exec;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
           
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                TempData["idlogin"] = HttpContext.Session.GetString("idLogin");
                TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
                TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
                TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            }
            else
            {
                HttpContext.Session.SetString("connectString", "Server=localhost;Database=Cinema;Trusted_Connection=True;");
            }
            List<Discount> listdis = new List<Discount>();
            string connectionString = HttpContext.Session.GetString("connectString");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string commandText = "EXEC dbo.USP_GetAllDiscount";

                var command = new SqlCommand(commandText, connection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Discount dis = new Discount();
                        dis.Id = Convert.ToString(reader[0]);
                        dis.Name = Convert.ToString(reader[1]);
                        dis.Description = Convert.ToString(reader[2]);
                        try
                        {
                            dis.PercentDiscount = Convert.ToInt32(reader[3]);
                        }
                        catch
                        {
                            dis.PercentDiscount = 0;
                        }
                        
                        //dis.MaxCost = Convert.ToInt32(reader[4]);
                        dis.MaxCost = reader[4].ToString() == "" ? 0 : int.Parse(reader[4].ToString());
                        dis.DateStart = Convert.ToDateTime(reader[5]);
                        dis.DateEnd = Convert.ToDateTime(reader[6]);
                        dis.ImageDiscount = Convert.ToString(reader[7]);
                        // dis.NoTicket = Convert.ToInt32(reader[8]);      
                        try
                        {
                            dis.Point = Convert.ToInt32(reader[8]);
                        }
                        catch
                        {
                            dis.Point = 0;
                        }
                        
                        dis.Used = Convert.ToInt32(reader[9]);
                        listdis.Add(dis);
                    }
                }
                catch (SqlException e)
                {
                    ModelState.AddModelError("", e.ToString());
                    return RedirectToAction("index", "home");
                }
                connection.Close();

            }
            ViewData["listDiscount"] = listdis;

            // get movies
            Exec = new ExecuteProcedure(connectionString);
            ViewBag.Movie = Exec.ExecuteGetMovieNow(0, 4).ToList();
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

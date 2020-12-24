using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModel;
namespace OnlineMoviesBooking.Controllers
{
    public class ContactViewController : Controller
    {
        private readonly CinemaContext _context;

        public ContactViewController(CinemaContext context)
        {
            _context = context;
        }
        public IActionResult AllQuestion()
        {
            if (HttpContext.Session.GetString("idLogin") != null)
            {
                TempData["idlogin"] = HttpContext.Session.GetString("idLogin");
                TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
                TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
                TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            }
            //var list = _context.Qa.FromSqlRaw("EXEC dbo.USP_GetAllQa").ToList();
            return View();
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
            //var list = _context.Qa.FromSqlRaw("EXEC dbo.USP_GetAllQa").ToList();
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateContactView([Bind("Email,Content")] ContactViewModel contactView)
        {
            TempData["idlogin"] = HttpContext.Session.GetString("idLogin");
            TempData["nameLogin"] = HttpContext.Session.GetString("nameLogin");
            TempData["imgLogin"] = HttpContext.Session.GetString("imgLogin");
            TempData["roleLogin"] = HttpContext.Session.GetString("roleLogin");
            if (ModelState.IsValid)
            {
                Qa qa = new Qa()
                {
                    Email = contactView.Email,
                    Content = contactView.Content,
                };
                qa.Id = Guid.NewGuid().ToString();
                qa.Time = DateTime.Now;
                string newpass = Guid.NewGuid().ToString().Substring(26);

                string connectionString;
                if (HttpContext.Session.GetString("idLogin") != null)
                {
                    string username = HttpContext.Session.GetString("idLogin");
                    string pw = HttpContext.Session.GetString("pwLogin");
                    connectionString = $"Server=localhost;Database=Cinema;MultipleActiveResultSets=true;User Id={username};Password={pw}";
                }
                else
                    connectionString = "Server=localhost;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string commandText = $"EXEC dbo.USP_InsertQa @id = '{qa.Id}',@email = '{qa.Email}', @time = '{qa.Time}', @content = N'{qa.Content}' ";

                    var command = new SqlCommand(commandText, connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        TempData["msg"] = "error";
                        return View(contactView);
                    }
                    connection.Close();
                }

                return RedirectToAction(nameof(Index));
            }
            return RedirectToActionPermanent("Index", "contactView");
        }
    }
}

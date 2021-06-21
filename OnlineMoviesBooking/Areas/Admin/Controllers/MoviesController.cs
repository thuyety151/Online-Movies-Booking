using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;                // image
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using OnlineMoviesBooking.Models.ViewModels;
using Microsoft.Data.SqlClient;
using System.Text.Encodings.Web;
using System.Text;
using System.Web.Helpers;

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly ExecuteProcedure Exec;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string check;

        public MoviesController(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            Exec = new ExecuteProcedure(httpContextAccessor.HttpContext.Session.GetString("connectString"));
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
            var movie = Exec.ExecuteMovieGetAll().Select(x => new            
            {
                id = x.Id,
                name = x.Name,
                releaseDate = x.ReleaseDate.GetValueOrDefault().ToString("dd-MM-yyyy"),
                runningtime = x.RunningTime.ToString(),
                poster = x.Poster
            });
            return Json(new { data = movie });
        }
        // GET: Movies
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
            return View();
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
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var movie = Exec.ExecuteMovieDetail(id);
                movie = DecodeData(movie);
                return View(movie);
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: Movies/Create
        [HttpGet]
        public IActionResult Upsert(string id=null)
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
            if (id != null)
            {
                // Edit
                try
                {
                    var movie = Exec.ExecuteMovieDetail(id);
                    ViewBag.Id = movie.Id;
                    return View(movie);
                }
                catch
                {
                    return NotFound();
                }
            }
            ViewBag.Id = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Movie movie, IFormFile files)
        {
            if (ModelState.IsValid)
            {
                //encode
                movie.Casts = HttpUtility.HtmlEncode(movie.Casts);
                movie.Description = HttpUtility.HtmlEncode(movie.Description);
                movie.Director = HttpUtility.HtmlEncode(movie.Director);
                movie.Genre = HttpUtility.HtmlEncode(movie.Genre);
                movie.Name = HttpUtility.HtmlEncode(movie.Name);
                if (movie.Rated != null)
                {
                    movie.Rated = HttpUtility.HtmlEncode(movie.Rated);
                }
                movie.Trailer = HttpUtility.HtmlEncode(movie.Trailer);

                
                // check image
                if (movie.Id==null && files == null)
                {
                    ModelState.AddModelError("Poster", "Thêm Poster");
                    
                    return View(movie);
                }
                // save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if(files!=null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\movies\");
                    var extension = Path.GetExtension(files.FileName); 

                    if(movie.Poster!=null)
                    {
                        // edit 
                        var imagePath = Path.Combine(wwwRootPath, movie.Poster.TrimStart('\\'));
                        if(System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files.CopyTo(filesStreams);
                    }
                    movie.Poster = @"\images\movies\" + fileName + extension;

                }
                else
                {
                    // update when do not change the image
                    if (movie.Id != null)
                    {
                        movie.Poster = Exec.ExecuteGetImageMovie(movie.Id);
                    }
                }


                if(movie.Rated==null)
                {
                    movie.Rated = "";
                }
                if (movie.Id == null)
                {
                    //Theem movie
                    movie.Id = Guid.NewGuid().ToString("N").Substring(0, 20);
                    string result = Exec.ExecuteInsertMovie(movie);
                    while (result.Contains("PRIMARY")) // trùng primary key do generate id
                    {
                        movie.Id = Guid.NewGuid().ToString("N").Substring(0, 20);
                        result = Exec.ExecuteInsertMovie(movie);

                    }
                    if (result != "")
                    {
                        ModelState.AddModelError("", result.ToString());
                        return View(movie);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // 
                    string result = Exec.ExecuteUpdateMovie(movie);
                    if (result != "")
                    {
                        ModelState.AddModelError("", result.ToString());
                        return View(movie);
                    }
                    return RedirectToAction(nameof(Index));
                }


            }
            if (movie.Id == null)
            {
                //Theem movie
                movie.Id = Guid.NewGuid().ToString("N").Substring(0, 20);
                string result = Exec.ExecuteInsertMovie(movie);
                while (result.Contains("PRIMARY")) // trùng primary key do generate id
                {
                    movie.Id = Guid.NewGuid().ToString("N").Substring(0, 20);
                    result = Exec.ExecuteInsertMovie(movie);

                }
                if (result != "")
                {
                    ModelState.AddModelError("", result.ToString());
                    return View(movie);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // 
                string result = Exec.ExecuteUpdateMovie(movie);
                if (result != "")
                {
                    ModelState.AddModelError("", result.ToString());
                    return View(movie);
                }
                return RedirectToAction(nameof(Index));
            }
        }
 
        // POST: Movies/Delete/5
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
            var movie = Exec.ExecuteMovieDetail(id);
            if(movie==null)
            {
                return Json(new { success = false });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, movie.Poster.TrimStart('\\'));
            if(System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            Exec.ExecuteDeleteMovie(id);

            return Json(new { success = true });
        }
        private Movie DecodeData(Movie movie)
        {
            movie.Casts = HttpUtility.HtmlDecode(movie.Casts);
            movie.Description = HttpUtility.HtmlDecode(movie.Description);
            movie.Director = HttpUtility.HtmlDecode(movie.Director);
            movie.Genre = HttpUtility.HtmlDecode(movie.Genre);
            movie.Name = HttpUtility.HtmlDecode(movie.Name);
            if (movie.Rated != null)
            {
                movie.Rated = HttpUtility.HtmlDecode(movie.Rated);
            }
            movie.Trailer = HttpUtility.HtmlDecode(movie.Trailer);
            return movie;
        }

    }
}

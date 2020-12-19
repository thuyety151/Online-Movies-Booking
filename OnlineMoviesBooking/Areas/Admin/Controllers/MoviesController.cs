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

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private ExecuteProcedure Exec;
        private readonly IWebHostEnvironment _hostEnvironment;
        public MoviesController(IWebHostEnvironment hostEnvironment,IHttpContextAccessor httpContextAccessor)
        {
            Exec = new ExecuteProcedure(httpContextAccessor.HttpContext.Session.GetString("connectString"));
            this._hostEnvironment = hostEnvironment;
        }

        public IActionResult GetAll()
        {
           // var obj = Exec.ExecuteMovieGetAll();        // dùng ajax chưa hiệu quả
            var movie = Exec.ExecuteMovieGetAll().Select(x => new             // tốn dữ liệu
            {
                id = x.Id,
                name = x.Name,
                releaseDate = x.ReleaseDate.ToString("dd-MM-yyyy"),
                runningtime = x.RunningTime.ToString(),
                poster = x.Poster
            });
            return Json(new { data = movie });
        }
        // GET: Movies
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var movie = Exec.ExecuteMovieDetail(id);
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
        public async Task<IActionResult> Upsert(Movie movie, IFormFile files)
        {
            
            if (ModelState.IsValid)
            {
                // check image
                if(movie.Id==null && files == null)
                {
                    ModelState.AddModelError("Poster", "Thêm Poster");
                    
                    return View(movie);
                }
                // save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                //var filess = HttpContext.Request.Form.Files;

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
                    string result= Exec.ExecuteInsertMovie(movie);
                    while(result.Contains("PRIMARY") ) // trùng primary key do generate id
                    {
                        movie.Id = Guid.NewGuid().ToString("N").Substring(0, 20);
                        result = Exec.ExecuteInsertMovie(movie);
                    }    

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // 
                    Exec.ExecuteUpdateMovie(movie);

                    return RedirectToAction(nameof(Index));
                }

            }
            return View(movie);
        }

 
        // POST: Movies/Delete/5
        [HttpDelete]
      
        public async Task<IActionResult> Delete(string id)
        {
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

    }
}

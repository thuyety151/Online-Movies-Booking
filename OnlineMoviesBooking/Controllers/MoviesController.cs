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

namespace OnlineMoviesBooking.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaContext _context;
        private ExecuteProcedure Exec;
        private readonly IWebHostEnvironment _hostEnvironment;
        public MoviesController(CinemaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            Exec = new ExecuteProcedure(context);
            this._hostEnvironment = hostEnvironment;
        }

        public IActionResult GetAll()
        {
           // var obj = Exec.ExecuteMovieGetAll();        // dùng ajax chưa hiệu quả
            var movie = Exec.ExecuteMovieGetAll().Select(x => new             // tốn dữ liệu
            {
                id = x.Id,
                name = x.Name,
                genre = x.Genre,
                director = x.Director,
                casts = x.Casts,
                rated = x.Rated,
                description = x.Description,
                trailer = x.Trailer,
                releaseDate = x.ReleaseDate.ToShortDateString(),
                expirationDate = x.ExpirationDate.ToShortDateString(),
                runningtime = x.RunningTime.ToString(),
                poster = x.Poster
            });
            return Json(new { data = movie });
        }
        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var movie = Exec.ExecuteMovieDetail(id);
            var obj = new
            {
                id = movie.Id,
                name = movie.Name,
                genre = movie.Genre,
                director = movie.Director,
                casts = movie.Casts,
                rated = movie.Rated,
                description = movie.Description,
                trailer = movie.Trailer,
                releaseDate = movie.ReleaseDate.ToShortDateString(),
                expirationDate = movie.ExpirationDate.ToShortDateString(),
                runningtime = movie.RunningTime.ToString(),
                poster = movie.Poster
            };
            return Json(new { data = obj});
        }

        // GET: Movies/Create
        public IActionResult Upsert(string? id)
        {
            if (id != null)
            {
                // Edit
                try
                {

                    var movie=Exec.ExecuteMovieDetail(id);
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
                    // update 
                    //if(movie.Id!=0)
                    //{
                        
                    //}
                }

               // ViewBag.FileName = movie.ImageFile.FileName;

                if(movie.Rated==null)
                {
                    movie.Rated = "";
                }
                // check validation
                if (Exec.CheckNameMovie(movie.Name) > 0)
                {
                    ModelState.AddModelError("Name", "Tên đã tồn tại");
                }
                if (movie.ReleaseDate > movie.ExpirationDate)
                {
                    ModelState.AddModelError("ExpirationDate", "Ngày không hợp lệ");
                }

                try
                {
                    if (movie.Id == null)
                    {
                        //Theem movie
                        movie.Id = Guid.NewGuid().ToString();
                        Exec.ExecuteInsertMovie(movie);

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // 
                        Exec.ExecuteUpdateMovie(movie);

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    return View(movie);
                }
               
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = Exec.ExecuteMovieDetail(id);
            if (movie == null)
            {

                return NotFound();
            }
            MovieViewModel m = new MovieViewModel(movie);
            return View(m);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Genre,Director,Casts,Rated,Description,Trailer,ReleaseDate,ExpirationDate,RunningTime,Poster")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var i = Exec.ExecuteUpdateMovie(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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

        private bool MovieExists(string id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}

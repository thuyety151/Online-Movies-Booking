﻿using System;
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
                releaseDate = x.ReleaseDate.ToShortDateString(),
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

            //var obj = new
            //{
            //    id = movie.Id,
            //    name = movie.Name,
            //    genre = movie.Genre,
            //    director = movie.Director,
            //    casts = movie.Casts,
            //    rated = movie.Rated,
            //    description = movie.Description,
            //    trailer = movie.Trailer,
            //    releaseDate = movie.ReleaseDate.ToShortDateString(),
            //    runningtime = movie.RunningTime.ToString(),
            //    poster = movie.Poster
            //};
            //return Json(new { data = obj});
            
        }

        // GET: Movies/Create
        [HttpGet]
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
            id = "";
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

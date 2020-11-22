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

namespace OnlineMoviesBooking.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaContext _context;
        private ExecuteProcedure Exec;

        public MoviesController(CinemaContext context)
        {
            _context = context;
            Exec = new ExecuteProcedure(context);
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
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie )
        {
            
            if (ModelState.IsValid)
            {
                movie.Id = Guid.NewGuid().ToString();
                if(movie.Rated==null)
                {
                    movie.Rated = "";
                }

                try
                {
                    if(Exec.CheckNameMovie(movie.Name)>0)
                    {
                        ModelState.AddModelError("Name", "Tên đã tồn tại");
                    }
                    if(movie.ReleaseDate>movie.ExpirationDate)
                    {
                        ModelState.AddModelError("ExpirationDate", "Ngày không hợp lệ");
                    }    
                    var i = Exec.ExecuteInsertMovie(movie);
                    return RedirectToAction(nameof(Index));
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
            return View(movie);
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
            Exec.ExecuteDeleteMovie(id);
            return Json(new { success = true });
        }

        private bool MovieExists(string id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}

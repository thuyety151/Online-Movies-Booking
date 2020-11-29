using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Controllers
{
    public class DiscountsController : Controller
    {
        private readonly CinemaContext _context;
        private ExecuteProcedure Exec;
        private readonly IWebHostEnvironment _hostEnvironment;
        public DiscountsController(CinemaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            Exec = new ExecuteProcedure(context);
            this._hostEnvironment = hostEnvironment;
        }

        public IActionResult GetAll()
        {
            var obj = Exec.ExecuteGetAllDiscount();
            return Json(new { data = obj });
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: Discounts/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = Exec.ExecuteGetDetailDiscount(id);

            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: Discounts/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discount discount, IFormFile files)
        {
            if (ModelState.IsValid)
            {
                // save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                //var filess = HttpContext.Request.Form.Files;

                if (files != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\discounts\");
                    var extension = Path.GetExtension(files.FileName);

                    if (discount.ImageDiscount != null)
                    {
                        // edit 
                        var imagePath = Path.Combine(wwwRootPath, discount.ImageDiscount.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files.CopyTo(filesStreams);
                    }
                    discount.ImageDiscount = @"\images\discounts\" + fileName + extension;

                }

                // gán các giá trị null để insert vào db
                discount.Id = Guid.NewGuid().ToString();
                discount.Used = 0;
                if(discount.MaxCost==null)
                {
                    discount.MaxCost = 0;
                }    
                if(discount.Point==null)
                {
                    discount.Point = 0;
                }

                string result = Exec.ExecuteInsertDiscount(discount);
                if(result=="")
                {
                    return RedirectToAction(nameof(Index));
                }
                if(result.Contains("Ngày không hợp lệ"))
                {
                    ModelState.AddModelError("DateEnd", "Ngày không hợp lệ");
                }      
            }

            return View(discount);
        }

        // GET: Discounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = Exec.ExecuteGetDetailDiscount(id);

            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Discount discount,IFormFile files)
        {
            if (id != discount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                //var filess = HttpContext.Request.Form.Files;

                if (files != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\discounts\");
                    var extension = Path.GetExtension(files.FileName);

                    if (discount.ImageDiscount != null)
                    {
                        // edit 
                        var imagePath = Path.Combine(wwwRootPath, discount.ImageDiscount.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files.CopyTo(filesStreams);
                    }
                    discount.ImageDiscount = @"\images\discounts\" + fileName + extension;

                }
                else
                {
                    // update when do not change the image
                    if (discount.Id != null)
                    {
                        discount.ImageDiscount = Exec.ExecuteGetImageDiscount(discount.Id);
                    }
                }

                string results = Exec.ExecuteUpdateDiscount(discount);
                if(results=="")
                {
                    return RedirectToAction(nameof(Index));
                }
                if (results.Contains("Ngày không hợp lệ"))
                {
                    ModelState.AddModelError("DateEnd", "Ngày không hợp lệ");
                }
            }
            discount.ImageDiscount = Exec.ExecuteGetImageDiscount(discount.Id);
            return View(discount);
        }

        // GET: Discounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discount
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var discount = await _context.Discount.FindAsync(id);
            _context.Discount.Remove(discount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(string id)
        {
            return _context.Discount.Any(e => e.Id == id);
        }
    }
}

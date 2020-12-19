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

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountsController : Controller
    {
        private ExecuteProcedure Exec;
        private readonly IWebHostEnvironment _hostEnvironment;
        public DiscountsController( IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
            Exec = new ExecuteProcedure(HttpContext.Session.GetString("connectString"));
        }

        public IActionResult GetAll()
        {
            var obj = Exec.ExecuteGetAllDiscount().Select(x => new
            {
                id=x.Id,
                name=x.Name,
                dateStart=x.DateStart,
                dateEnd= x.DateEnd,
                imageDiscount = x.ImageDiscount,
                used=x.Used

            });
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
                discount.Id = Guid.NewGuid().ToString("N").Substring(0, 10);
                discount.Used = 0;

                string result = Exec.ExecuteInsertDiscount(discount);
                while (result.Contains("PRIMARY"))
                {
                    discount.Id = Guid.NewGuid().ToString("N").Substring(0, 10);
                    result = Exec.ExecuteInsertDiscount(discount);
                }
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
        public IActionResult Edit(string id)
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

        [HttpDelete]
        public IActionResult Delete(string id)
        { 
            // xóa hình trong file
            var image = Exec.ExecuteGetImageDiscount(id);

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(wwwRootPath, image.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            Exec.ExecuteDeleteDiscount(id);
            
            return Json(new { success = true });
        }

    }
}

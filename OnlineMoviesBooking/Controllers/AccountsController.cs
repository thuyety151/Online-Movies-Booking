using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Controllers
{
    public class AccountsController : Controller
    {
        private readonly CinemaContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AccountsController(CinemaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            //var cinemaContext = _context.Account.Include(a => a.IdTypesOfUserNavigation);
            
            var list = _context.Account.FromSqlRaw("EXEC dbo.USP_SelectAllAccount").ToList();
            return View( list);
        }

        //public IActionResult GetAll()
        //{
        //    var list = _context.Account.FromSqlRaw("EXEC dbo.SelectAccount").ToList();
        //    return Json(new { data = list });
        //}
        // GET: Accounts/Details/5

        public IActionResult Get(string id)
        {
            try
            {
                var account = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList();    // 
                return Json(new { data = account });
            }
            catch (Exception e)
            {

                return Json(new { data = e.Message });


            }
        }
        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["IdTypeOfMember"] = new SelectList(_context.TypeOfMember, "IdTypeMember", "TypeOfMemberName");
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id","Name","Birthdate","Gender","Address","Sdt","Email","Password","Point", "IdTypesOfUser", "IdTypeOfMember","Image")] Account account, IFormFile files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    
                    if(files != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"image\Account\");
                        var extension = Path.GetExtension(files.FileName);

                        if (account.Image != null)
                        {
                            // edit 
                            var imagePath = Path.Combine(wwwRootPath, account.Image.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }

                        }
                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            files.CopyTo(filesStreams);
                        }
                        account.Image = @"\image\Account\" + fileName + extension;
                    }    
                        

                    account.Id = Guid.NewGuid().ToString();
                    //var sqlParam = new SqlParameter[]
                    //{
                    //    new SqlParameter("@id",account.Id),
                    //    new SqlParameter("@name",account.Name),
                    //    new SqlParameter("@birthdate",account.Birthdate),
                    //    new SqlParameter("@gender",account.Gender),
                    //    new SqlParameter("@address",account.Address),
                    //    new SqlParameter("@SDT",account.Sdt),
                    //    new SqlParameter("@Email",account.Email),
                    //    new SqlParameter("@password",account.Password),
                    //    new SqlParameter("@point",account.Point),
                    //    new SqlParameter("@usertypeid",account.IdTypesOfUser),
                    //    new SqlParameter("@membertypeid",account.IdTypeOfMember),
                    //    new SqlParameter("@image",account.Image),
                    //    new SqlParameter("@id","Insert")
                    //};
                    _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertUpdateAccount @id = {account.Id},@name = {account.Name},@birthdate = {account.Birthdate},@gender={account.Gender},@address={account.Address},@SDT={account.Sdt},@Email={account.Email},@password={account.Password},@point ={account.Point},@usertypeid={account.IdTypesOfUser},@membertypeid = {account.IdTypeOfMember}, @image={account.Image},@action ={"Insert"} ");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                var modelstateKey = ModelState.Keys;
                var list = _context.Account.FromSqlRaw($"EXEC dbo.USP_CheckEmail '{account.Email}' ").ToList();
                if (list.Count() == 1)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                }
                //else if (check_db == 1)
                //{
                //    ModelState.AddModelError("Sdt", "Sdt đã tồn tại");
                //}
                ViewData["IdTypeOfMember"] = new SelectList(_context.TypeOfMember, "IdTypeMember", "TypeOfMemberName");
                ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
                return View();
            }
            ViewData["IdTypeOfMember"] = new SelectList(_context.TypeOfMember, "IdTypeMember", "TypeOfMemberName");
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["IdTypeOfMember"] = new SelectList(_context.TypeOfMember, "IdTypeMember", "TypeOfMemberName");
            ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Name", account.IdTypesOfUser);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IFormFile files, [Bind("Id", "Name", "Birthdate", "Gender", "Address", "Sdt", "Email", "Password", "Point", "IdTypesOfUser", "IdTypeOfMember", "Image")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    if (files != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"image\Account\");
                        var extension = Path.GetExtension(files.FileName);

                        if (account.Image != null)
                        {
                            // edit 
                            var imagePath = Path.Combine(wwwRootPath, account.Image.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }

                        }
                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            files.CopyTo(filesStreams);
                        }
                        account.Image = @"\image\Account\" + fileName + extension;
                    }
                    _context.Database.ExecuteSqlCommand($"EXEC dbo.USP_InsertUpdateAccount @id = {account.Id},@name = {account.Name},@birthdate = {account.Birthdate},@gender={account.Gender},@address={account.Address},@SDT={account.Sdt},@Email={account.Email},@password={account.Password},@point ={account.Point},@usertypeid={account.IdTypesOfUser},@membertypeid = {account.IdTypeOfMember}, @image={account.Image},@action ={"Update"} ");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            ViewData["IdTypeOfMember"] = new SelectList(_context.TypeOfMember, "IdTypeMember", "TypeOfMemberName");
            //ViewData["IdTypesOfUser"] = new SelectList(_context.TypesOfAccount, "Id", "Id", account.IdTypesOfUser);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public IActionResult Delete(string id)
        {
            try
            {
                
                var image = _context.Account.FromSqlRaw($"EXEC dbo.USP_GetDetailAccount @id = '{id}'").ToList()[0].Image;
                System.IO.File.Delete(image);

                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (image != null)
                {
                    var imagePath = Path.Combine(wwwRootPath, image.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }    
                _context.Account.FromSqlRaw($"EXEC dbo.USP_DeleteAccount @id = {id}");
                return Json(new { success = true, message = "Xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}

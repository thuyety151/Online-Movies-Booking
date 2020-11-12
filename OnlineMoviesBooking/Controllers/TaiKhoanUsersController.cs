using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.Models.Models.DB;

namespace OnlineMoviesBooking.Controllers
{
    public class TaiKhoanUsersController : Controller
    {
        private readonly cinemaContext _context;

        public TaiKhoanUsersController(cinemaContext context)
        {
            _context = context;
        }

        // GET: TaiKhoanUsers
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.TaiKhoan.Include(t => t.IdKhuyenMaiNavigation).Include(t => t.Usertype);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: TaiKhoanUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoan
                .Include(t => t.IdKhuyenMaiNavigation)
                .Include(t => t.Usertype)
                .FirstOrDefaultAsync(m => m.IdTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoanUsers/Create
        public IActionResult Create()
        {
            ViewData["IdKhuyenMai"] = new SelectList(_context.KhuyenMai, "IdKhuyenMai", "IdKhuyenMai");
            ViewData["UsertypeId"] = new SelectList(_context.TypeUser, "UsertypeId", "UsertypeId");
            return View();
        }

        // POST: TaiKhoanUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTaiKhoan,TenKhachHang,NgaySinh,GioiTinh,DiaChi,Sdt,Email,UserName,Password,UsertypeId,IdKhuyenMai")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taiKhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdKhuyenMai"] = new SelectList(_context.KhuyenMai, "IdKhuyenMai", "IdKhuyenMai", taiKhoan.IdKhuyenMai);
            ViewData["UsertypeId"] = new SelectList(_context.TypeUser, "UsertypeId", "UsertypeId", taiKhoan.UsertypeId);
            return View(taiKhoan);
        }

        // GET: TaiKhoanUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoan.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            ViewData["IdKhuyenMai"] = new SelectList(_context.KhuyenMai, "IdKhuyenMai", "IdKhuyenMai", taiKhoan.IdKhuyenMai);
            ViewData["UsertypeId"] = new SelectList(_context.TypeUser, "UsertypeId", "UsertypeId", taiKhoan.UsertypeId);
            return View(taiKhoan);
        }

        // POST: TaiKhoanUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTaiKhoan,TenKhachHang,NgaySinh,GioiTinh,DiaChi,Sdt,Email,UserName,Password,UsertypeId,IdKhuyenMai")] TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.IdTaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.IdTaiKhoan))
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
            ViewData["IdKhuyenMai"] = new SelectList(_context.KhuyenMai, "IdKhuyenMai", "IdKhuyenMai", taiKhoan.IdKhuyenMai);
            ViewData["UsertypeId"] = new SelectList(_context.TypeUser, "UsertypeId", "UsertypeId", taiKhoan.UsertypeId);
            return View(taiKhoan);
        }

        // GET: TaiKhoanUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoan
                .Include(t => t.IdKhuyenMaiNavigation)
                .Include(t => t.Usertype)
                .FirstOrDefaultAsync(m => m.IdTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // POST: TaiKhoanUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taiKhoan = await _context.TaiKhoan.FindAsync(id);
            _context.TaiKhoan.Remove(taiKhoan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(int id)
        {
            return _context.TaiKhoan.Any(e => e.IdTaiKhoan == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;

namespace OnlineMoviesBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BillsController : Controller
    {
        private readonly CinemaContext _context;
        private ExecuteProcedure Exec;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BillsController(CinemaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            Exec = new ExecuteProcedure(context);
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Bills
        public ActionResult GetAll()
        {
            var bill = Exec.ExecuteGetAllBill().Select(x => new
            {
                idAccount = x.IdAccount,
                idShow = x.IdShow,
                idSeat = x.IdSeat,
                accountName = x.AccountName,
                movieName = x.MovieName,
                timeStart = x.TimeStart,
                row = x.Row,
                no = x.No,
                totalPrice = x.TotalPrice.ToString(),
                date = x.Date.ToShortDateString(),
                status = x.Status.ToString(),
                code = x.Code,
            });
            return Json(new { data = bill });
        }
        public async Task<IActionResult> Index()
        {
            return View();
            //var cinemaContext = _context.Bill.Include(b => b.IdAccountNavigation).Include(b => b.IdSeatNavigation).Include(b => b.IdShowNavigation);
            //List<Bill> lstBill = new List<Bill>();
            //using (SqlConnection con = new SqlConnection(cs))
            //{
            //    con.Open();
            //    // TêN STORE
            //    SqlCommand com = new SqlCommand("USP_GetAllBill", con);
            //    com.CommandType = CommandType.StoredProcedure;
            //    SqlDataReader rdr = com.ExecuteReader();

            //    while (rdr.Read())
            //    {
            //        lstBill.Add(new Bill
            //        {
            //            IdAccount = rdr["Id_Account"].ToString(),
            //            IdShow = rdr["Id_Show"].ToString(),
            //            IdSeat = rdr["Id_Seat"].ToString(),
            //            Date = DateTime.Parse(rdr["Date"].ToString()),
            //            TotalPrice = int.Parse(rdr["TotalPrice"].ToString()),
            //            Status = bool.Parse(rdr["Status"].ToString()),
            //        });

            //    }
            //}
            //return View(lstBill);
        }

        //GET: Admin/Bills/Details/
        public async Task<IActionResult> Details(string idaccount, string idshow, string idseat)
        {
            if (idaccount == null || idseat == null || idshow == null)
            {
                return NotFound();
            }
            var bill = Exec.ExecuteGetDetailBill(idaccount, idshow, idseat);
                if (idaccount == null || idshow == null || idseat == null)
                {
                    return NotFound();
                }
                return View(bill);

        }

        // GET: Admin/Bills/Create
        public IActionResult Create()
        {
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id");
            ViewData["IdSeat"] = new SelectList(_context.Seat, "Id", "Id");
            ViewData["IdShow"] = new SelectList(_context.Show, "Id", "Id");
            return View();
        }

        // POST: Admin/Bills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSeat,IdAccount,IdShow,Date,TotalPrice,Code,Status")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id", bill.IdAccount);
            ViewData["IdSeat"] = new SelectList(_context.Seat, "Id", "Id", bill.IdSeat);
            ViewData["IdShow"] = new SelectList(_context.Show, "Id", "Id", bill.IdShow);
            return View(bill);
        }

        // GET: Admin/Bills/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bill.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id", bill.IdAccount);
            ViewData["IdSeat"] = new SelectList(_context.Seat, "Id", "Id", bill.IdSeat);
            ViewData["IdShow"] = new SelectList(_context.Show, "Id", "Id", bill.IdShow);
            return View(bill);
        }

        // POST: Admin/Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdSeat,IdAccount,IdShow,Date,TotalPrice,Code,Status")] Bill bill)
        {
            if (id != bill.IdSeat)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillExists(bill.IdSeat))
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
            ViewData["IdAccount"] = new SelectList(_context.Account, "Id", "Id", bill.IdAccount);
            ViewData["IdSeat"] = new SelectList(_context.Seat, "Id", "Id", bill.IdSeat);
            ViewData["IdShow"] = new SelectList(_context.Show, "Id", "Id", bill.IdShow);
            return View(bill);
        }

        // GET: Admin/Bills/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bill
                .Include(b => b.IdAccountNavigation)
                .Include(b => b.IdSeatNavigation)
                .Include(b => b.IdShowNavigation)
                .FirstOrDefaultAsync(m => m.IdSeat == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: Admin/Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var bill = await _context.Bill.FindAsync(id);
            _context.Bill.Remove(bill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillExists(string id)
        {
            return _context.Bill.Any(e => e.IdSeat == id);
        }
    }
}

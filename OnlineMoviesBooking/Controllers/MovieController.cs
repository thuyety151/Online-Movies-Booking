﻿using BraintreeHttp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OnlineMoviesBooking.DataAccess.Data;
using OnlineMoviesBooking.Models.Models;
using OnlineMoviesBooking.Models.ViewModels;
using PayPal.Core;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMoviesBooking.Controllers
{
    public class MovieController : Controller
    {
        private readonly string _clientId;
        private readonly string _secretKey;
        private ExecuteProcedure Exec;
        public MovieController(IConfiguration config)
        {
            Exec = new ExecuteProcedure();
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }
        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult ComingSoon(int? page)
        {
            List<Movie> lstmovie = new List<Movie>();
            int num = 2;
            var movieCount = Exec.GetCountMovieComing();
            if (movieCount % num == 0)
            {
                ViewBag.AllPage = movieCount / num;
            }
            else
            {
                ViewBag.AllPage = movieCount / num + 1;
            }
            // điều kiện để phân trang
            if (page != null && page > 0 && num < movieCount)
            {

                /*số trang có thể có:
                 *TH1: ví dụ có 18 sản phẩm, mỗi trang chứa 9 sản phẩm thì có TỐI ĐA 2 TRANG
                 *TH2: ví dụ có 20 sản phẩm, vậy có tối đa 3 trang và trang cuối chứa 2 sản phẩm 
               */
                int numpage = movieCount % num == 0 ? movieCount / num : (movieCount / num) + 1;

                if (page > numpage)
                {
                    ViewBag.page = numpage;
                }
                else
                {
                    ViewBag.page = page;
                }
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= numpage && movieCount % num != 0)
                {

                    // lấy số lẻ
                    lstmovie = Exec.ExecuteGetMovieComingSoon(num * (numpage - 1), movieCount % num);

                }
                //trường hợp 1
                else
                {

                    lstmovie = Exec.ExecuteGetMovieComingSoon(num * (page.GetValueOrDefault() - 1), num);
                }



            }
            else
            {
                ViewBag.page = 1;
                lstmovie = Exec.ExecuteGetMovieComingSoon(0, num);
            }
            return View(lstmovie);
        }
        public IActionResult Now(int? page)
        {
            List<Movie> lstmovie = new List<Movie>();
            int num = 2;
            var movieCount = Exec.GetCountMovieNow();
            if (movieCount % num == 0)
            {
                ViewBag.AllPage = movieCount / num;
            }
            else
            {
                ViewBag.AllPage = movieCount / num + 1;
            }
            // điều kiện để phân trang
            if (page != null && page > 0 && num < movieCount)
            {

                /*số trang có thể có:
                 *TH1: ví dụ có 18 sản phẩm, mỗi trang chứa 9 sản phẩm thì có TỐI ĐA 2 TRANG
                 *TH2: ví dụ có 20 sản phẩm, vậy có tối đa 3 trang và trang cuối chứa 2 sản phẩm 
               */
                int numpage = movieCount % num == 0 ? movieCount / num : (movieCount / num) + 1;

                if (page > numpage)
                {
                    ViewBag.page = numpage;
                }
                else
                {
                    ViewBag.page = page;
                }
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= numpage && movieCount % num != 0)
                {

                    // lấy số lẻ
                     lstmovie =Exec.ExecuteGetMovieNow(num * (numpage - 1), movieCount%num);

                }
                //trường hợp 1
                else
                {

                     lstmovie = Exec.ExecuteGetMovieNow(num * (page.GetValueOrDefault() - 1), num);
                }



            }
            else
            {
                ViewBag.page = 1;
                 lstmovie = Exec.ExecuteGetMovieNow(0, num);
            }
            return View(lstmovie);
        }
        public IActionResult Detail(string id)
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
        [HttpGet]
        public IActionResult ShowsDetail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // tìm phim 
            var movie = Exec.ExecuteMovieDetail(id);

            ViewBag.Id = id;
            // rạp option
            var theater = Exec.ExecuteTheaterGetAll();
            ViewBag.Theater = theater;

            // date option
            DateTime now = DateTime.Now;
            int temp = 0;
            List<string> dateshow = new List<string>();
             while(temp!=7)
            {
               
                dateshow.Add(now.Date.ToString("dd/MM/yyyy"));
                // now.Date.ToString("") + "/" + now.Date.ToString("MM") + "/" + now.Date.ToString("yyyy")
                now = now.AddDays(1);
                temp++;
            }
            ViewBag.Date = dateshow;
            
            return View(movie);
        }
        
        public IActionResult SeatPlan(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var plan = Exec.ExecuteGetDetailShow(id);
            if (plan == null)
            {
                return NotFound();
            }

            Exec.ExecDeleteBillStatus0("1");

            ViewBag.IdShow = id;
            ViewBag.MovieName = plan.MovieName;
            ViewBag.ScreenName = plan.ScreenName;
            ViewBag.TheaterName = plan.TheaterName;
            ViewBag.TimeStart = plan.TimeStart.ToString("HH:mm");
            ViewBag.Date = plan.TimeStart.ToString("dd/mm/yyyy");

            var allseat = Exec.ExecGetAllSeat(plan.Id);
            ViewBag.RowA = allseat.Where(x => x.Row == "A").ToList();
            ViewBag.RowB = allseat.Where(x => x.Row == "B").ToList();
            ViewBag.RowC = allseat.Where(x => x.Row == "C").ToList();
            ViewBag.RowD = allseat.Where(x => x.Row == "D").ToList();
            ViewBag.RowE = allseat.Where(x => x.Row == "E").ToList();
            ViewBag.RowF = allseat.Where(x => x.Row == "F").ToList();
            ViewBag.RowG = allseat.Where(x => x.Row == "G").ToList();
            ViewBag.RowH = allseat.Where(x => x.Row == "H").ToList();

            return View();
        }
       


       // ============================ Json
        public IActionResult ShowsDate(DateTime date)
        {
            if(date==null)
            {
                return NotFound();
            }
            var show = Exec.ExecuteGetAllShowDate(date);
            return Json(new { data = show });
        }
        

        [HttpGet]
        public IActionResult getshowbydate(string idMovie, string date)
        {
            if(idMovie==null || date == null)
            {
                return NotFound();
            }
            DateTime d = DateTime.Parse(date);
            // can co them ten rap
            var theater = Exec.ExecuteFindTheaterShow(idMovie, d.ToString("yyyy-MM-dd"));
            // tìm tên, id các rạp thỏa điều kiện
            return Json(  theater );
        }
        [HttpGet]
        public IActionResult getprice()
        {
            //List<TypesOfSeat> price = new List<TypesOfSeat>();
             var price = Exec.GetAllTypesOfSeat();
            return Json(price);
        }
        [HttpGet]
        public IActionResult TimeOut()
        {
            return RedirectToAction("Index");
        }

        public IActionResult PayPal()
        {
            return View();
        }
        [HttpGet]
        public IActionResult getinfo(string idshow, string lstSeat)
        {
            // Kiểm tra ghế và lịch hợp lệ => gán vào BillViewModel : nếu xác nhận bill sẽ add vào database
            // Kiểm tra ID show hợp lêk
            var show = Exec.ExecuteGetDetailShow(idshow);
            List<string> lstseat = lstSeat.Split(' ').ToList();
            if (show == null)
            {
                return Json(new { success = false });
            }
            if (lstSeat == " undefined")
            {
                return Json(new { success = false });
            }
            foreach (var item in lstseat.ToList())
            {
                if (item == "undefined" || item == "")
                {
                    lstseat.Remove(item);
                }
            }
            if (lstseat.Count == 0)
            {
                return Json(new { success = false });
            }
            var seatVM = new List<string>();
            foreach (var item in lstseat)
            {
                seatVM.Add(Exec.ExecCheckIdSeat(item, idshow).Id);
            }

            foreach (var item in seatVM)
            {
                if (item == null)
                {
                    return Json(false);
                }
            }
           
            return Json(seatVM );
        }
        [HttpGet]
        public IActionResult CheckOut(string idshow,string lstSeat)
        {
            List<string> lst = lstSeat.Split(',').ToList();

            // tạo bill
            List<string> seats = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                if (lst.Count <= i)
                {
                    seats.Add(null);
                }
                else
                {
                    seats.Add(lst[i]);
                }
            }
            string result = Exec.ExecInsertBill(seats, "1", idshow, null);
            // chưa xử lí transaction

            // get bill
            var bill = Exec.ExecGetBillDetail("1", idshow);
            ViewBag.Show = idshow;
            return View(bill);
        }
        [HttpGet]
        public IActionResult UseDiscount( string code)
        {
            
            var obj = Exec.ExecUseDiscount("1", code);

            return Json(obj);
        }
        public IActionResult TimeOut(string idshow)
        {
            Exec.ExecDeleteBillStatus0("1");
            return View("Index");
        }
        public async System.Threading.Tasks.Task<IActionResult> PaypalCheckout(string iddiscount)
        {
            if (iddiscount != null)
            {
                // áp dụng khuyến mãi
                Exec.ExecAddDiscount("1",iddiscount);
            }
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            var checkout = Exec.TestCheckout("1");

            #region Create Paypal Order
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            double total = 0;
            if (checkout.TotalPer != null)
            {

                total = Math.Round(double.Parse(checkout.TotalPer.ToString()) / 23000, 2);
            }
            else
            {
                total = Math.Round(double.Parse(checkout.TotalCost.ToString()) / 23000, 2);
            }

            // 0 đ không cân thanh toán paypal
            if (total == 0)
            {
                return RedirectToAction("CheckoutSuccess");
            }

            itemList.Items.Add(new Item()
            {
                Name = checkout.Name,
                Currency = "USD",
                Price = total.ToString(),
                Quantity = "1",
                Description = "No: " + checkout.No
            });
            #endregion

            //var total = Math.Round(double.Parse(checkout[0].Total.ToString()) / 23000, 2) *itemList.Items.Count;

            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",

                        },
                        ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/Movie/CheckoutFail",
                    ReturnUrl = $"{hostname}/Movie/CheckoutSuccess"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("/Movie/CheckoutFail");
            }
        }
        public IActionResult CheckoutFail()
        {
            //Tạo đơn hàng trong database với trạng thái thanh toán là "Chưa thanh toán"
            //Xóa session
            Exec.ExecDeleteBillStatus0("1");
            return Content("Thanh toán không thành công");
        }

        public IActionResult CheckoutSuccess()
        {
            //Tạo đơn hàng trong database với trạng thái thanh toán là "Paypal" và thành công
            //Xóa session
            Exec.ExecUpdateBillStatus("1");
            return Content("Thanh toán thành công");
        }
    }
}

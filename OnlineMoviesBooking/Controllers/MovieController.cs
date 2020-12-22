using BraintreeHttp;
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
        private readonly ExecuteProcedure Exec;
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

            Exec.ExecDeleteTicketStatus0("1");

            ViewBag.IdShow = id;
            ViewBag.MovieName = plan.MovieName;
            ViewBag.ScreenName = plan.ScreenName;
            ViewBag.TheaterName = plan.TheaterName;
            ViewBag.Language = plan.Languages;
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
        public IActionResult Getshowbydate(string idMovie, string date)
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
        public IActionResult Getprice()
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
        public IActionResult Getinfo(string idshow, string lstSeat)
        {
            // Kiểm tra ghế và lịch hợp lệ => gán vào BillViewModel : nếu xác nhận bill sẽ add vào database
            // Kiểm tra ID show hợp lêk
            var show = Exec.ExecuteGetDetailShow(idshow);
            List<string> lstseat = lstSeat.Split(' ').ToList();
            if (show == null)
            {
                return Json("error");    // show không hợp lệ
            }
            if (lstSeat == " undefined")
            {
                return Json("seat");    // ghế chọn không hợp lệ
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
                return Json("seat");  // ghế chọn không hợp lệ
            }
            var seatVM = new List<string>();
            foreach (var item in lstseat)
            {
                seatVM.Add(Exec.ExecCheckIdSeat(item, idshow).Id);  // kiểm tra idseat là hợp lệ
            }

            foreach (var item in seatVM)
            {
                if (item == null)
                {
                    return Json("error");
                }
            }
            // tạo 8 ghế để insert vào tickets
            List<string> seats = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                if (seatVM.Count <= i)
                {
                    seats.Add(null);
                }
                else
                {
                    seats.Add(seatVM[i]);
                }
            }
            // insert seat vào ticket
            string result = Exec.ExecInsertTickets(seats, "1", idshow, null);
            //  xử lí transaction
            if (result == "Ghế đã được chọn")
            {
                return Json(result);
            }
            return Json(seatVM );
        }
        [HttpGet]
        public IActionResult CheckOut(string idshow,string lstSeat)
        {
            

            // get bill
                var bill = Exec.ExecGetTicketDetail("1", idshow);
            ViewBag.Show = idshow;
            return View(bill);
        }
        [HttpGet]
        public IActionResult UseDiscount( string code)
        {
            
            var obj = Exec.ExecUseDiscount("1", code);

            return Json(obj);
        }
        [HttpGet]
        public IActionResult UsePoint(string point)
        {
            // kiểm tra điểm hợp lệ
            var obj = Exec.ExecCheckPoint("1", int.Parse(point));
            return Json(obj);
        }
        public IActionResult TimeOut(string idshow)
        {
            string s=Exec.ExecDeleteTicketStatus0("1");
            if (s != "")
            {
                // co loi xay ra
                return Content("Đã xảy ra lỗi trong quá trình hủy bill");
            }
            return View("Index");
        }
        public async System.Threading.Tasks.Task<IActionResult> PaypalCheckout(string code,string pointuse)
        {
            double total = 0;
            if (code != null)
            {
                // áp dụng khuyến mãi
                Exec.ExecAddDiscount("1", code);
            }
            
            if (pointuse != null)
            {
                // add point vào bill và trừ ở account
                // sẽ tran khi thanh toán ko thành công
                string execPoint = Exec.ExecAddPoint("1", pointuse);
                if(execPoint== "Không dùng được point")
                {
                    // lỗi điểm âm sau khi trừ
                    return Content("Điểm dùng không hợp lệ");
                }
                total -= int.Parse(pointuse) * 1000;
            }
            else
            {
                pointuse = "0";
            }
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);

            var checkout = Exec.TestCheckout("1",pointuse);
            // GET POINT
            int point;
            if (checkout.PointCost == null)
            {
                point = checkout.PointPer.GetValueOrDefault();
            }
            else
            {
                point = checkout.PointCost.GetValueOrDefault();
            }

            #region Create Paypal Order
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            
            if (checkout.TotalPer != null)
            {

                total = Math.Round(double.Parse((checkout.TotalPer- int.Parse(pointuse)*1000).ToString()) / 23000, 2);
            }
            else
            {
                total += Math.Round(double.Parse((checkout.TotalCost - int.Parse(pointuse)*1000).ToString()) / 23000, 2);
            }

            // 0 đ không cân thanh toán paypal
            if (total == 0)
            {
                return RedirectToAction("CheckoutSuccess");
            }

            itemList.Items.Add(new Item()
            {
                Name = checkout.MovieName,
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
                    ReturnUrl = $"{hostname}/Movie/CheckoutSuccess?point="+point
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
            string s = Exec.ExecDeleteTicketStatus0("1");
            if (s != "")
            {
                // co loi xay ra
                return Content("Đã xảy ra lỗi trong quá trình hủy bill");
            }
            return Content("Thanh toán không thành công");
        }

        public IActionResult CheckoutSuccess(int point=0)
        {
            //Tạo đơn hàng trong database với trạng thái thanh toán là "Paypal" và thành công
            //Xóa session
            
            Exec.ExecUpdateTicketStatus("1",point);
            return Content("Thanh toán thành công");
        }
    }
}

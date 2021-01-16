using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class ViewBillAdminModel
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string MovieName { get; set; }
        public string ScreenName { get; set; }
        public string TheaterName { get; set; }
        public DateTime TimeStart { get; set; }
        public string Row { get; set; }
        public int No { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public string DiscountName { get; set; }
        public int Point { get; set; }

    }
}

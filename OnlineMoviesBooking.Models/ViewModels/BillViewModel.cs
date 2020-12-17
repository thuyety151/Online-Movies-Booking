using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class BillViewModel
    {
        public List<string> IdSeat { get; set; }
        public string IdAccount { get; set; }
        public string IdShow { get; set; }
        public string Code { get; set; }
        public int No { get; set; }
        public double TotalPrice { get; set; }
        public string MovieName { get; set; }
        public int RunningTime { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public string TheaterName { get; set; }
        public string ScreenName { get; set; }
        public string Languages { get; set; }
        public string Address { get; set; }
    }
}

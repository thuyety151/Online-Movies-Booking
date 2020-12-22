using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public string MovieName { get; set; }
        public string Name { get; set; }
        public string Languages { get; set; }
        public string IdShow { get; set; }
        public  DateTime TimeStart { get; set; }
        public  DateTime TimeEnd { get; set; }
        public  int? TotalPer { get; set; }
        public  int? PointPer { get; set; }
        public  int? TotalCost { get; set; }
        public  int? PointCost { get; set; }
        public string TheaterName { get; set; }
        public string ScreenName { get; set; }
        public string No { get; set; }
        public string RunningTime { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
    }
}

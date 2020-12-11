using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public string IdMovie { get; set; }
        public string Name { get; set; }
        public string Languages { get; set; }
        public string IdShow { get; set; }
        public  DateTime TimeStart { get; set; }
        public  int Total { get; set; }
        public string TheaterName { get; set; }
        public string ScreenName { get; set; }
        public string No { get; set; }
    }
}

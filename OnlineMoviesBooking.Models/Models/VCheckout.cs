using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class VCheckout
    {
        public string IdMovie { get; set; }
        public string Name { get; set; }
        public string Languages { get; set; }
        public string IdShow { get; set; }
        public DateTime TimeStart { get; set; }
        public string TheaterName { get; set; }
        public string ScreenName { get; set; }
        public int? Total { get; set; }
        public int? No { get; set; }
    }
}

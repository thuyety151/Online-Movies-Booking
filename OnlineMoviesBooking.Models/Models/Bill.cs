using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Bill
    {
        public string IdSeat { get; set; }
        public string IdAccount { get; set; }
        public string IdShow { get; set; }
        public DateTime Date { get; set; }
        public int TotalPrice { get; set; }
        public string Code { get; set; }
        public int? Point { get; set; }

        public virtual Account IdAccountNavigation { get; set; }
        public virtual Seat IdSeatNavigation { get; set; }
        public virtual Show IdShowNavigation { get; set; }
    }
}

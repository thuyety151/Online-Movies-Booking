using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Ticket
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int? Point { get; set; }
        public bool? Status { get; set; }
        public string IdShow { get; set; }
        public string IdAccount { get; set; }
        public string IdDiscount { get; set; }
        public string IdSeat { get; set; }

        public virtual Account IdAccountNavigation { get; set; }
        public virtual Discount IdDiscountNavigation { get; set; }
        public virtual Seat IdSeatNavigation { get; set; }
        public virtual Show IdShowNavigation { get; set; }
    }
}

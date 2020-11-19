using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class UseDiscount
    {
        public string IdDiscount { get; set; }
        public string IdAccount { get; set; }
        public bool Status { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Code { get; set; }

        public virtual Account IdAccountNavigation { get; set; }
        public virtual Discount IdDiscountNavigation { get; set; }
    }
}

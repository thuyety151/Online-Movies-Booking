using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class VPrice
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public int? Point { get; set; }
        public bool? Status { get; set; }
        public string IdShow { get; set; }
        public string IdAccount { get; set; }
        public string IdDiscount { get; set; }
        public string IdSeat { get; set; }
        public int Cost { get; set; }
    }
}

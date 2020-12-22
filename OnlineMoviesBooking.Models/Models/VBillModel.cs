using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class VBillModel
    {
        public string IdShow { get; set; }
        public string IdAccount { get; set; }
        public int? No { get; set; }
        public int? TotalPrice { get; set; }
        public string MovieName { get; set; }
        public int RunningTime { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string TheaterName { get; set; }
        public string ScreenName { get; set; }
        public string Languages { get; set; }
        public string Address { get; set; }
        public bool? Status { get; set; }
        public DateTime? Date { get; set; }
        public string IdDiscount { get; set; }
        public int? Point { get; set; }
    }
}

using OnlineMoviesBooking.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public partial class BillAdminViewModel
    {
        public string IdAccount { get; set; }
        public string IdShow { get; set; }
        public string IdSeat { get; set; }
        public string AccountName { get; set; }
        public string MovieName { get; set; }
        public DateTime TimeStart { get; set; }
        public string Row { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public int TotalPrice { get; set; }
        public bool? Status { get; set; }
        public string Code { get; set; }
    }
}

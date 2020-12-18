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
        public DateTime Date { get; set; }
        public int TotalPrice { get; set; }
        public string Code { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class ShowtoBillViewModel
    {
        public string IdShow { get; set; }
        public string Languages { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string IdMovie { get; set; }
        public string IdScreen { get; set; }
        // note
        public List<string> lstIdSeat { get; set; }
        public string IdAccount { get; set; }
        public DateTime Date { get; set; }
        public int TotalPrice { get; set; }
        public string Code { get; set; }
    }
}

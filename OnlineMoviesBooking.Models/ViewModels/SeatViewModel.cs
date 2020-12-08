using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class SeatViewModel
    {
        public string Id { get; set; }
        public string IdTypesOfSeat { get; set; }
        public string IdScreen { get; set; }
        public string Row { get; set; }
        public int No { get; set; }
        public string Status { get; set; }
    }
    
}

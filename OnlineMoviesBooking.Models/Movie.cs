using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models
{
    public class Movie
    {
        public int IdMovie { get; set; }
        public int Name { get; set; }
        public string Directors { get; set; }
        public string Cast { get; set; }
        public string Description { get; set; }
        public string LinkTrailer { get; set; }
        public DateTime ReleasdDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int RunningTime { get; set; }
        public string Rated { get; set; }
    }
}

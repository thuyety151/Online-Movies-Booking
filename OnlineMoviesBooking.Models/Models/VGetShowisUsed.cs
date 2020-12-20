using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class VGetShowisUsed
    {
        public string Id { get; set; }
        public string Languages { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string MovieName { get; set; }
        public string Poster { get; set; }
        public string ScreenName { get; set; }
        public string TheaterName { get; set; }
    }
}

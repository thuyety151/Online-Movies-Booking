using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class VMovieNow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Casts { get; set; }
        public string Rated { get; set; }
        public string Description { get; set; }
        public string Trailer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int RunningTime { get; set; }
        public string Poster { get; set; }
    }
}

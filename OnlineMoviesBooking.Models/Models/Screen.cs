using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Screen
    {
        public Screen()
        {
            Seat = new HashSet<Seat>();
            Show = new HashSet<Show>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IdTheater { get; set; }

        public virtual Theater IdTheaterNavigation { get; set; }
        public virtual ICollection<Seat> Seat { get; set; }
        public virtual ICollection<Show> Show { get; set; }
    }
}

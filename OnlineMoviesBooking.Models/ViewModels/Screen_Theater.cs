using OnlineMoviesBooking.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class Screen_Theater
    {
        public Screen_Theater()
        {

        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IdTheater { get; set; }
        public string NameTheater { get; set; }
        public virtual Theater IdTheaterNavigation { get; set; }
        public virtual ICollection<Seat> Seat { get; set; }
        public virtual ICollection<Show> Show { get; set; }
    }
}

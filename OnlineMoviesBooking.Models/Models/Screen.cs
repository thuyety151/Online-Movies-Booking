using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Tên phòng chiếu")]
        [Required(ErrorMessage ="Vui lòng nhập tên phòng chiếu")]
        public string Name { get; set; }
        [Display(Name = "Rạp chiếu")]
        public string IdTheater { get; set; }

        public virtual Theater IdTheaterNavigation { get; set; }
        public virtual ICollection<Seat> Seat { get; set; }
        public virtual ICollection<Show> Show { get; set; }
    }
}

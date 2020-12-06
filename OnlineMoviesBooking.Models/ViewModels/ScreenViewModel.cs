using OnlineMoviesBooking.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class ScreenViewModel
    {
        public string Id { get; set; }
        [Display(Name="Tên phòng chiếu")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Rạp chiếu")]
        [Required]
        public string IdTheater { get; set; }

        public virtual Theater IdTheaterNavigation { get; set; }
        public virtual ICollection<Seat> Seat { get; set; }
        public virtual ICollection<Show> Show { get; set; }
    }
}

﻿using System;
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
        [Display(Name = "Tên rạp")]
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

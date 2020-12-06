using OnlineMoviesBooking.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class ThreaterViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên rạp chiếu phim")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required]
        public string Address { get; set; }
        [Required]
        public string Hotline { get; set; }

        public virtual ICollection<Screen> Screen { get; set; }
    }
}

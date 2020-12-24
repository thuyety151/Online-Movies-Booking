using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Theater
    {
        public Theater()
        {
            Screen = new HashSet<Screen>();
        }

        public string Id { get; set; }
        [Display(Name="Tên rạp chiếu phim")]
        [Required(ErrorMessage ="Vui lòng nhập tên rạp chiếu phim")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Hotline")]
        [Required(ErrorMessage = "Vui lòng nhập hotline")]
        public string Hotline { get; set; }

        public virtual ICollection<Screen> Screen { get; set; }
    }
}

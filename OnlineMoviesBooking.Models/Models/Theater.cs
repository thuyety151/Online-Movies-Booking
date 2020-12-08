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
        public string Name { get; set; }
        [Display(Name="Địa chỉ")]
        public string Address { get; set; }

        public string Hotline { get; set; }

        public virtual ICollection<Screen> Screen { get; set; }
    }
}

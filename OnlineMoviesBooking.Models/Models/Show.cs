using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Show
    {
        public Show()
        {
            Bill = new HashSet<Bill>();
        }

        public string Id { get; set; }

        [Display(Name = "Ngôn ngữ")]
        public string Languages { get; set; }
        [Display(Name = "Giờ bắt đầu")]
        [Required]
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        [Display(Name = "Tên phim")]
        [Required]
        public string IdMovie { get; set; }
        [Display(Name = "Tên phòng chiếu")]
        [Required]
        public string IdScreen { get; set; }

        public virtual Movie IdMovieNavigation { get; set; }
        public virtual Screen IdScreenNavigation { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
    }
}

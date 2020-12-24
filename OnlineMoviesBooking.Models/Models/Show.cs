using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Show
    {
        public Show()
        {
            Ticket = new HashSet<Ticket>();
        }

        public string Id { get; set; }
        [Display(Name = "Ngôn ngữ")]
        public string Languages { get; set; }
        [Display(Name = "Thời gian bắt đầu")]
        [Required(ErrorMessage ="Thời gian bắt đầu")]
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        [Display(Name = "Phim")]
        
        public string IdMovie { get; set; }
        [Display(Name = "Phòng chiếu")]
        public string IdScreen { get; set; }

        public virtual Movie IdMovieNavigation { get; set; }
        public virtual Screen IdScreenNavigation { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}

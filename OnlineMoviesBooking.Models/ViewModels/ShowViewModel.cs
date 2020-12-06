using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModels
{
    public class ShowViewModel
    {
        public string Id { get; set; }
        [Display(Name="Ngôn ngữ")]
        public string Languages { get; set; }

        [Display(Name ="Giờ bắt đầu")]
        [Required]
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }

        [Display(Name ="Tên phim")]
        [Required]
        public string IdMovie { get; set; }

        [Display(Name ="Tên phòng chiếu")]
        [Required]
        public string IdScreen { get; set; }
        [Display(Name = "Tên phòng chiếu")]
        [Required]
        public string IdTheater { get; set; }

        [Display(Name = "Phim")]
        public string MovieName { get; set; }

        [Display(Name = "Phòng chiếu")]
        public string ScreenName { get; set; }

        [Display(Name = "Rạp")]
        public string TheaterName { get; set; }
        public string Poster { get; set; }
    }
}

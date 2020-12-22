using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Show = new HashSet<Show>();
        }

        public string Id { get; set; }
        [Display(Name = "Tên phim")]
        [Required(ErrorMessage ="Vui lòng nhập tên phim")]
        public string Name { get; set; }
        [Display(Name = "Thể loại")]
        [Required(ErrorMessage = "Vui lòng nhập thể loại phim")]
        public string Genre { get; set; }
        [Display(Name = "Đạo diễn")]
        [Required(ErrorMessage = "Vui lòng nhập tên đạo diễn")]
        public string Director { get; set; }
        [Display(Name = "Diễn viên")]
        [Required(ErrorMessage = "Vui lòng nhập tên diễn viên")]
        public string Casts { get; set; }
        [Display(Name = "Qui định")]
        public string Rated { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; }
        [Display(Name = "Link trailer")]
        [Required(ErrorMessage = "Vui lòng nhập link trailer")]
        public string Trailer { get; set; }
        [Display(Name = "Ngày khởi chiếu")]
        [Required(ErrorMessage = "Vui lòng chọn ngày khởi chiếu")]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "Thời lượng")]
        [Required(ErrorMessage = "Vui lòng nhập thời lượng")]
        public int RunningTime { get; set; }
        [Display(Name = "Poster")]
        public string Poster { get; set; }


        public virtual ICollection<Show> Show { get; set; }
    }
}

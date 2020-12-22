using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class TypesOfSeat
    {
        public TypesOfSeat()
        {
            Seat = new HashSet<Seat>();
        }

        public string Id { get; set; }
        [Display(Name="Tên loại ghế")]
        [Required(ErrorMessage = "Vui lòng nhập tên loại ghế")]
        public string Name { get; set; }
        [Display(Name="Giá")]
        [Required(ErrorMessage ="Vui lòng nhập giá")]
        [Range(minimum:0, maximum:1000000000,ErrorMessage ="Dữ liệu không hợp lệ")]
        public int Cost { get; set; }
        public int? Num { get; set; }

        public virtual ICollection<Seat> Seat { get; set; }
    }
}

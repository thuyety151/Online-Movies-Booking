using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Ticket = new HashSet<Ticket>();
            UseDiscount = new HashSet<UseDiscount>();
        }

        public string Id { get; set; }
        [Display(Name="Tên")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Phần trăm khuyến mãi")]
        [Range(minimum:0,maximum:100,ErrorMessage ="Phần trăm không hợp lệ")]
        public int? PercentDiscount { get; set; }
        [Display(Name = "Thành tiền (VNĐ)")]
        public int? MaxCost { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [Required]
        public DateTime? DateStart { get; set; }
        [Display(Name = "Ngày kết thúc")]
        [Required]
        public DateTime? DateEnd { get; set; }
        [Display(Name = "Hình ảnh")]
        public string ImageDiscount { get; set; }
        public int? Point { get; set; }
        [Display(Name = "Số lượng đã sử dụng")]
        public int? Used { get; set; }

        public string Code { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<UseDiscount> UseDiscount { get; set; }
    }
}

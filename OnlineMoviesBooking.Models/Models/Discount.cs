using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Discount
    {
        public Discount()
        {
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
        public int PercentDiscount { get; set; }
        [Display(Name = "Mức giảm tối đa")]
        public int? MaxCost { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? DateStart { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public DateTime? DateEnd { get; set; }
        [Display(Name = "Hình ảnh")]
        [Required]
        public string ImageDiscount { get; set; }
        [Display(Name = "Số lượng vé / lần áp dụng")]
        [Required]
        public int? NoTicket { get; set; }
        [Display(Name = "Số điểm quy đổi")]
        public int? Point { get; set; }
        [Display(Name = "Số lượng đã sử dụng")]
        public int? Used { get; set; }

        public virtual ICollection<UseDiscount> UseDiscount { get; set; }
    }
}

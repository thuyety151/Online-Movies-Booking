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
        [Required(ErrorMessage = "Vui lòng nhập tên khuyến mãi")]
        
        public string Name { get; set; }
        [Display(Name = "Code")]
        [Required(ErrorMessage = "Vui lòng nhập Code khuyến mãi")]
        public string Code { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; }
        [Display(Name = "Phần trăm giảm (%)")]
        [Range(minimum:0 , maximum:100)]
        public int? PercentDiscount { get; set; }
        [Display(Name = "Mức giảm tối đa (VNĐ)")]
        [Range(minimum:0,maximum:1000000000)]
        public int? MaxCost { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        public DateTime DateStart { get; set; }
        [Display(Name = "Ngày kết thúc")]
        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Hình ảnh")]
        public string ImageDiscount { get; set; }
        public int? Point { get; set; }
        [Display(Name="Số lượng dùng")]
        public int? Used { get; set; }
        


        public virtual ICollection<UseDiscount> UseDiscount { get; set; }
    }
}

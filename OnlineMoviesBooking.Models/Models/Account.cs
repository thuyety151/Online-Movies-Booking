using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class Account
    {
        public Account()
        {
            Bill = new HashSet<Bill>();
            Qa = new HashSet<Qa>();
            UseDiscount = new HashSet<UseDiscount>();
        }

        public string Id { get; set; }
        [Required(ErrorMessage ="Dữ liệu không được để trống!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống!")]
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        [Required]
        public string Sdt { get; set; }
        [EmailAddress(ErrorMessage ="Dữ liệu bạn vừa nhập không là email")]
        [Required (ErrorMessage = "Dữ liệu không được để trống!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống!")]
        [MinLength(7,ErrorMessage ="Mật khẩu phải có ít nhất 7 ký tự")]
        public string Password { get; set; }
        public int Point { get; set; }
        public string IdTypesOfUser { get; set; }
        public string Image { get; set; }

        public virtual TypesOfAccount IdTypesOfUserNavigation { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
        public virtual ICollection<Qa> Qa { get; set; }
        public virtual ICollection<UseDiscount> UseDiscount { get; set; }
    }
}

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
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public DateTime Birthdate { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public bool Gender { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string Address { get; set; }     
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [Phone(ErrorMessage = "Dữ liệu không hợp lệ")]
        [MinLength(9, ErrorMessage = "Dữ liệu không hợp lệ")]
        public string Sdt { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [EmailAddress(ErrorMessage = "Dữ liệu không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [MinLength(7, ErrorMessage = "Dữ liệu không hợp lệ")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [Range(0,1000000000,ErrorMessage ="Dữ liệu không hợp lệ")]
        public int Point { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string IdTypesOfUser { get; set; }
        public string IdTypeOfMember { get; set; }
        public string Image { get; set; }

        public virtual TypesOfAccount IdTypesOfUserNavigation { get; set; }
        public virtual ICollection<Bill> Bill { get; set; }
        public virtual ICollection<Qa> Qa { get; set; }
        public virtual ICollection<UseDiscount> UseDiscount { get; set; }
    }
}

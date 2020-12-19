using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModel
{
    public class AccountViewModel
    {
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
        [Range(0, 1000000000, ErrorMessage = "Dữ liệu không hợp lệ")]
        public int Point { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string IdTypesOfUser { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string IdTypeOfMember { get; set; }
        public IFormFile Image { get; set; }
    }
}

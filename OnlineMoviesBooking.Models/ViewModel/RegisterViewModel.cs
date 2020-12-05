using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [EmailAddress(ErrorMessage = "Dữ liệu không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [MinLength(7, ErrorMessage = "Dữ liệu không hợp lệ")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [Compare("Password",ErrorMessage ="Mật khẩu không khớp")]
        public string PasswordConfirm { get; set; }
    }
}

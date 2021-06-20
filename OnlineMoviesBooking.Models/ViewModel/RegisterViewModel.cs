using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid username address")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]

        public string FullName { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]

        public string SDT { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{7,}", ErrorMessage = "Your password must be at least 7 characters long and contain at least 1 letter and 1 number")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string PasswordConfirm { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]

        public string FullName { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]

        public string SDT { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        public string PasswordConfirm { get; set; }
    }
}

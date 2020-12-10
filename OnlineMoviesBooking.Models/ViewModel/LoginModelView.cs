using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModel
{
    public class LoginModelView
    {
        [Required]
        [Range(7,20, ErrorMessage = "Dữ liệu không hợp lệ")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Dữ liệu không được để trống")]
        [MinLength(7, ErrorMessage = "Dữ liệu không hợp lệ")]
        public string Password { get; set; }
    }
}

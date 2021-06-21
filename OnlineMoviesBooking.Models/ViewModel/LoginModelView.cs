using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModel
{
    public class LoginModelView
    {
        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid username address")]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{7,}", ErrorMessage = "Your password must be at least 7 characters long and contain at least 1 letter and 1 number")]
        public string Password { get; set; }
    }
}

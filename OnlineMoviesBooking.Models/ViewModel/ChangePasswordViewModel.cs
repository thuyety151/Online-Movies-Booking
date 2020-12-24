using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineMoviesBooking.Models.ViewModel
{
    public class ChangePasswordViewModel
    {

        public string OldPasswword { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword",ErrorMessage =@"Mật khẩu nhập lại không khớp!!")]
        public string ComfirmNewPassword { get; set; }
    }
}

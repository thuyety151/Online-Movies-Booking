using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class Aq
    {
        public int IdTaikhoan { get; set; }
        public string Email { get; set; }
        public DateTime Aqtime { get; set; }
        public string Noidung { get; set; }

        public virtual TaiKhoan IdTaikhoanNavigation { get; set; }
    }
}

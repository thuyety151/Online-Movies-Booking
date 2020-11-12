using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.DB
{
    public partial class ThanhVien
    {
        public int IdThanhVien { get; set; }
        public int? Diem { get; set; }

        public virtual TaiKhoan IdThanhVienNavigation { get; set; }
    }
}

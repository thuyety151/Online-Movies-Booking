using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.DB
{
    public partial class LoaiTaiKhoan
    {
        public LoaiTaiKhoan()
        {
            TaiKhoan = new HashSet<TaiKhoan>();
        }

        public int IdLoaiTk { get; set; }
        public string TenLoaiTk { get; set; }

        public virtual ICollection<TaiKhoan> TaiKhoan { get; set; }
    }
}

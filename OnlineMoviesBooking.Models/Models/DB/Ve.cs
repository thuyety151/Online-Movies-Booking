using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class Ve
    {
        public int IdVe { get; set; }
        public string TheLoai { get; set; }
        public int? IdLichChieu { get; set; }
        public int? IdTaiKhoan { get; set; }
        public int? IdLoaiVe { get; set; }

        public virtual LichChieu IdLichChieuNavigation { get; set; }
        public virtual LoaiVe IdLoaiVeNavigation { get; set; }
        public virtual TaiKhoan IdTaiKhoanNavigation { get; set; }
    }
}

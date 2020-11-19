using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class KhuyenMai
    {
        public KhuyenMai()
        {
            TaiKhoan = new HashSet<TaiKhoan>();
        }

        public int IdKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; }
        public string NoiDung { get; set; }
        public string Mota { get; set; }

        public virtual ICollection<TaiKhoan> TaiKhoan { get; set; }
    }
}

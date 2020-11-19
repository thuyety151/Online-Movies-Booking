using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class Phim
    {
        public Phim()
        {
            LichChieu = new HashSet<LichChieu>();
        }

        public int IdPhim { get; set; }
        public string TenPhim { get; set; }
        public string TheLoaiPhim { get; set; }
        public string DaoDien { get; set; }
        public string QuiDinh { get; set; }
        public string MoTaPhim { get; set; }
        public string Trailor { get; set; }
        public DateTime? NgayKhoiChieu { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string Poster { get; set; }

        public virtual ICollection<LichChieu> LichChieu { get; set; }
    }
}

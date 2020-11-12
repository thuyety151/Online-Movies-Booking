using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class Rap
    {
        public Rap()
        {
            LichChieu = new HashSet<LichChieu>();
            PhongChieu = new HashSet<PhongChieu>();
        }

        public int IdRap { get; set; }
        public string TenRap { get; set; }
        public string DiaChi { get; set; }
        public string Hotline { get; set; }

        public virtual ICollection<LichChieu> LichChieu { get; set; }
        public virtual ICollection<PhongChieu> PhongChieu { get; set; }
    }
}

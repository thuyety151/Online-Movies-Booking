using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.DB
{
    public partial class Ghe
    {
        public int IdGhe { get; set; }
        public string TenGhe { get; set; }
        public bool? TrangThai { get; set; }
        public int? IdPhong { get; set; }

        public virtual PhongChieu IdPhongNavigation { get; set; }
    }
}

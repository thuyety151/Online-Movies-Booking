using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class LichChieu
    {
        public LichChieu()
        {
            Ve = new HashSet<Ve>();
        }

        public int IdLichChieu { get; set; }
        public string NgonNgu { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public int? IdPhim { get; set; }
        public int? IdRap { get; set; }

        public virtual Phim IdPhimNavigation { get; set; }
        public virtual Rap IdRapNavigation { get; set; }
        public virtual ICollection<Ve> Ve { get; set; }
    }
}

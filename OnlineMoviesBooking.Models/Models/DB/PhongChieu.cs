using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.DB
{
    public partial class PhongChieu
    {
        public PhongChieu()
        {
            Ghe = new HashSet<Ghe>();
        }

        public int IdPhong { get; set; }
        public int? Idrap { get; set; }
        public string TenPhong { get; set; }

        public virtual Rap IdrapNavigation { get; set; }
        public virtual ICollection<Ghe> Ghe { get; set; }
    }
}

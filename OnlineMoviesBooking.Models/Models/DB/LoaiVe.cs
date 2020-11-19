using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class LoaiVe
    {
        public LoaiVe()
        {
            Ve = new HashSet<Ve>();
        }

        public int IdLoaiVe { get; set; }
        public string TenLoaiVe { get; set; }
        public int? GiaVe { get; set; }

        public virtual ICollection<Ve> Ve { get; set; }
    }
}

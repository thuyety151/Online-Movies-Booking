using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class Quyen
    {
        public Quyen()
        {
            UsertypeQuyen = new HashSet<UsertypeQuyen>();
        }

        public int QuyenId { get; set; }
        public string Tenquyen { get; set; }

        public virtual ICollection<UsertypeQuyen> UsertypeQuyen { get; set; }
    }
}

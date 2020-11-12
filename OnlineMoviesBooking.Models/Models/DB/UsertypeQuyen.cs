using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class UsertypeQuyen
    {
        public int UsertypeId { get; set; }
        public int QuyenId { get; set; }
        public string GhiChu { get; set; }

        public virtual Quyen Quyen { get; set; }
        public virtual TypeUser Usertype { get; set; }
    }
}

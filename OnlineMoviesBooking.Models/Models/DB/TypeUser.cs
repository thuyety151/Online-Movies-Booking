using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class TypeUser
    {
        public TypeUser()
        {
            TaiKhoan = new HashSet<TaiKhoan>();
            UsertypeQuyen = new HashSet<UsertypeQuyen>();
        }

        public int UsertypeId { get; set; }
        public string UsertypeName { get; set; }

        public virtual ICollection<TaiKhoan> TaiKhoan { get; set; }
        public virtual ICollection<UsertypeQuyen> UsertypeQuyen { get; set; }
    }
}

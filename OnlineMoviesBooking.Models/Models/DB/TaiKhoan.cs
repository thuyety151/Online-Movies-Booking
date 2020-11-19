﻿using System;
using System.Collections.Generic;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class TaiKhoan
    {
        public TaiKhoan()
        {
            Aq = new HashSet<Aq>();
            Ve = new HashSet<Ve>();
        }

        public int IdTaiKhoan { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool? GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? UsertypeId { get; set; }
        public int? IdKhuyenMai { get; set; }

        public virtual KhuyenMai IdKhuyenMaiNavigation { get; set; }
        public virtual TypeUser Usertype { get; set; }
        public virtual ICollection<Aq> Aq { get; set; }
        public virtual ICollection<Ve> Ve { get; set; }
    }
}
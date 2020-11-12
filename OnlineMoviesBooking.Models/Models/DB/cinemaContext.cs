using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnlineMoviesBooking.Models.Models.DB
{
    public partial class cinemaContext : DbContext
    {
        public cinemaContext()
        {
        }

        public cinemaContext(DbContextOptions<cinemaContext> options)
            : base(options)
        {
           
        }

        public virtual DbSet<Aq> Aq { get; set; }
        public virtual DbSet<Ghe> Ghe { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMai { get; set; }
        public virtual DbSet<LichChieu> LichChieu { get; set; }
        public virtual DbSet<LoaiVe> LoaiVe { get; set; }
        public virtual DbSet<Phim> Phim { get; set; }
        public virtual DbSet<PhongChieu> PhongChieu { get; set; }
        public virtual DbSet<Quyen> Quyen { get; set; }
        public virtual DbSet<Rap> Rap { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
        public virtual DbSet<TypeUser> TypeUser { get; set; }
        public virtual DbSet<UsertypeQuyen> UsertypeQuyen { get; set; }
        public virtual DbSet<Ve> Ve { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=THANHTON;Database=cinema;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aq>(entity =>
            {
                entity.HasKey(e => new { e.IdTaikhoan, e.Aqtime });

                entity.ToTable("AQ");

                entity.Property(e => e.IdTaikhoan).HasColumnName("id_taikhoan");

                entity.Property(e => e.Aqtime)
                    .HasColumnName("AQtime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Noidung)
                    .HasColumnName("noidung")
                    .HasMaxLength(1000);

                entity.HasOne(d => d.IdTaikhoanNavigation)
                    .WithMany(p => p.Aq)
                    .HasForeignKey(d => d.IdTaikhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AQ__id_taikhoan__33D4B598");
            });

            modelBuilder.Entity<Ghe>(entity =>
            {
                entity.HasKey(e => e.IdGhe);

                entity.HasIndex(e => e.TenGhe)
                    .HasName("UQ__Ghe__32A386F1A6D35A83")
                    .IsUnique();

                entity.Property(e => e.IdGhe).ValueGeneratedNever();

                entity.Property(e => e.TenGhe)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPhongNavigation)
                    .WithMany(p => p.Ghe)
                    .HasForeignKey(d => d.IdPhong)
                    .HasConstraintName("FK__Ghe__IdPhong__2A4B4B5E");
            });

            modelBuilder.Entity<KhuyenMai>(entity =>
            {
                entity.HasKey(e => e.IdKhuyenMai);

                entity.Property(e => e.IdKhuyenMai)
                    .HasColumnName("Id_KhuyenMai")
                    .ValueGeneratedNever();

                entity.Property(e => e.Mota).HasMaxLength(255);

                entity.Property(e => e.NoiDung).HasMaxLength(100);

                entity.Property(e => e.TenKhuyenMai).HasMaxLength(255);
            });

            modelBuilder.Entity<LichChieu>(entity =>
            {
                entity.HasKey(e => e.IdLichChieu);

                entity.Property(e => e.IdLichChieu).ValueGeneratedNever();

                entity.Property(e => e.NgonNgu).HasMaxLength(20);

                entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime");

                entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");

                entity.HasOne(d => d.IdPhimNavigation)
                    .WithMany(p => p.LichChieu)
                    .HasForeignKey(d => d.IdPhim)
                    .HasConstraintName("FK__LichChieu__IdPhi__31EC6D26");

                entity.HasOne(d => d.IdRapNavigation)
                    .WithMany(p => p.LichChieu)
                    .HasForeignKey(d => d.IdRap)
                    .HasConstraintName("FK__LichChieu__IdRap__32E0915F");
            });

            modelBuilder.Entity<LoaiVe>(entity =>
            {
                entity.HasKey(e => e.IdLoaiVe);

                entity.Property(e => e.IdLoaiVe).ValueGeneratedNever();

                entity.Property(e => e.TenLoaiVe).HasMaxLength(50);
            });

            modelBuilder.Entity<Phim>(entity =>
            {
                entity.HasKey(e => e.IdPhim);

                entity.Property(e => e.IdPhim).ValueGeneratedNever();

                entity.Property(e => e.DaoDien).HasMaxLength(50);

                entity.Property(e => e.MoTaPhim).HasMaxLength(255);

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

                entity.Property(e => e.NgayKhoiChieu).HasColumnType("datetime");

                entity.Property(e => e.Poster)
                    .HasColumnName("poster")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.QuiDinh).HasMaxLength(255);

                entity.Property(e => e.TenPhim).HasMaxLength(100);

                entity.Property(e => e.TheLoaiPhim).HasMaxLength(50);

                entity.Property(e => e.Trailor).HasMaxLength(255);
            });

            modelBuilder.Entity<PhongChieu>(entity =>
            {
                entity.HasKey(e => e.IdPhong);

                entity.Property(e => e.IdPhong).ValueGeneratedNever();

                entity.Property(e => e.Idrap).HasColumnName("IDRap");

                entity.Property(e => e.TenPhong).HasMaxLength(50);

                entity.HasOne(d => d.IdrapNavigation)
                    .WithMany(p => p.PhongChieu)
                    .HasForeignKey(d => d.Idrap)
                    .HasConstraintName("FK__PhongChie__IDRap__29572725");
            });

            modelBuilder.Entity<Quyen>(entity =>
            {
                entity.Property(e => e.QuyenId)
                    .HasColumnName("quyen_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Tenquyen)
                    .HasColumnName("tenquyen")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Rap>(entity =>
            {
                entity.HasKey(e => e.IdRap);

                entity.Property(e => e.IdRap).ValueGeneratedNever();

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Hotline)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TenRap).HasMaxLength(50);
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.IdTaiKhoan)
                    .HasName("PK_TK");

                entity.HasIndex(e => new { e.Email, e.UserName })
                    .HasName("UNIQUE_TK")
                    .IsUnique();

                entity.Property(e => e.IdTaiKhoan)
                    .HasColumnName("Id_TaiKhoan")
                    .ValueGeneratedNever();

                entity.Property(e => e.DiaChi).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdKhuyenMai).HasColumnName("Id_KhuyenMai");

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TenKhachHang).HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsertypeId).HasColumnName("usertype_id");

                entity.HasOne(d => d.IdKhuyenMaiNavigation)
                    .WithMany(p => p.TaiKhoan)
                    .HasForeignKey(d => d.IdKhuyenMai)
                    .HasConstraintName("FK__TaiKhoan__Id_Khu__2B3F6F97");

                entity.HasOne(d => d.Usertype)
                    .WithMany(p => p.TaiKhoan)
                    .HasForeignKey(d => d.UsertypeId)
                    .HasConstraintName("FK__TaiKhoan__userty__2C3393D0");
            });

            modelBuilder.Entity<TypeUser>(entity =>
            {
                entity.HasKey(e => e.UsertypeId)
                    .HasName("PK_usertype");

                entity.Property(e => e.UsertypeId)
                    .HasColumnName("usertype_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.UsertypeName)
                    .HasColumnName("usertype_name")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<UsertypeQuyen>(entity =>
            {
                entity.HasKey(e => new { e.UsertypeId, e.QuyenId })
                    .HasName("PK_userquyen");

                entity.ToTable("Usertype_quyen");

                entity.Property(e => e.UsertypeId).HasColumnName("usertype_id");

                entity.Property(e => e.QuyenId).HasColumnName("quyen_id");

                entity.Property(e => e.GhiChu).HasMaxLength(255);

                entity.HasOne(d => d.Quyen)
                    .WithMany(p => p.UsertypeQuyen)
                    .HasForeignKey(d => d.QuyenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Usertype___quyen__2E1BDC42");

                entity.HasOne(d => d.Usertype)
                    .WithMany(p => p.UsertypeQuyen)
                    .HasForeignKey(d => d.UsertypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Usertype___usert__2D27B809");
            });

            modelBuilder.Entity<Ve>(entity =>
            {
                entity.HasKey(e => e.IdVe);

                entity.Property(e => e.IdVe)
                    .HasColumnName("Id_Ve")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdLichChieu).HasColumnName("Id_LichChieu");

                entity.Property(e => e.TheLoai).HasMaxLength(50);

                entity.HasOne(d => d.IdLichChieuNavigation)
                    .WithMany(p => p.Ve)
                    .HasForeignKey(d => d.IdLichChieu)
                    .HasConstraintName("FK__Ve__Id_LichChieu__2F10007B");

                entity.HasOne(d => d.IdLoaiVeNavigation)
                    .WithMany(p => p.Ve)
                    .HasForeignKey(d => d.IdLoaiVe)
                    .HasConstraintName("FK__Ve__IdLoaiVe__30F848ED");

                entity.HasOne(d => d.IdTaiKhoanNavigation)
                    .WithMany(p => p.Ve)
                    .HasForeignKey(d => d.IdTaiKhoan)
                    .HasConstraintName("FK__Ve__IdTaiKhoan__300424B4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

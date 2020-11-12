using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnlineMoviesBooking.Models.DB
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

        public virtual DbSet<Ghe> Ghe { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMai { get; set; }
        public virtual DbSet<LichChieu> LichChieu { get; set; }
        public virtual DbSet<LoaiTaiKhoan> LoaiTaiKhoan { get; set; }
        public virtual DbSet<LoaiVe> LoaiVe { get; set; }
        public virtual DbSet<Phim> Phim { get; set; }
        public virtual DbSet<PhongChieu> PhongChieu { get; set; }
        public virtual DbSet<Rap> Rap { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
        public virtual DbSet<ThanhVien> ThanhVien { get; set; }
        public virtual DbSet<Ve> Ve { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=THANHTON;Database=cinema;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ghe>(entity =>
            {
                entity.HasKey(e => e.IdGhe);

                entity.Property(e => e.IdGhe).ValueGeneratedNever();

                entity.Property(e => e.TenGhe)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPhongNavigation)
                    .WithMany(p => p.Ghe)
                    .HasForeignKey(d => d.IdPhong)
                    .HasConstraintName("FK__Ghe__IdPhong__5070F446");
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
                    .HasConstraintName("FK__LichChieu__IdPhi__571DF1D5");

                entity.HasOne(d => d.IdRapNavigation)
                    .WithMany(p => p.LichChieu)
                    .HasForeignKey(d => d.IdRap)
                    .HasConstraintName("FK__LichChieu__IdRap__5812160E");
            });

            modelBuilder.Entity<LoaiTaiKhoan>(entity =>
            {
                entity.HasKey(e => e.IdLoaiTk)
                    .HasName("PK_LoaiTK");

                entity.Property(e => e.IdLoaiTk)
                    .HasColumnName("IdLoaiTK")
                    .ValueGeneratedNever();

                entity.Property(e => e.TenLoaiTk)
                    .HasColumnName("TenLoaiTK")
                    .HasMaxLength(10);
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
                    .HasConstraintName("FK__PhongChie__IDRap__4F7CD00D");
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

                entity.Property(e => e.IdTaiKhoan)
                    .HasColumnName("Id_TaiKhoan")
                    .ValueGeneratedNever();

                entity.Property(e => e.DiaChi).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdKhuyenMai).HasColumnName("Id_KhuyenMai");

                entity.Property(e => e.IdLoaiTk).HasColumnName("Id_LoaiTK");

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

                entity.HasOne(d => d.IdKhuyenMaiNavigation)
                    .WithMany(p => p.TaiKhoan)
                    .HasForeignKey(d => d.IdKhuyenMai)
                    .HasConstraintName("FK__TaiKhoan__Id_Khu__52593CB8");

                entity.HasOne(d => d.IdLoaiTkNavigation)
                    .WithMany(p => p.TaiKhoan)
                    .HasForeignKey(d => d.IdLoaiTk)
                    .HasConstraintName("FK__TaiKhoan__Id_Loa__5165187F");
            });

            modelBuilder.Entity<ThanhVien>(entity =>
            {
                entity.HasKey(e => e.IdThanhVien);

                entity.Property(e => e.IdThanhVien).ValueGeneratedNever();

                entity.HasOne(d => d.IdThanhVienNavigation)
                    .WithOne(p => p.ThanhVien)
                    .HasForeignKey<ThanhVien>(d => d.IdThanhVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThanhVien__IdTha__5629CD9C");
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
                    .HasConstraintName("FK__Ve__Id_LichChieu__534D60F1");

                entity.HasOne(d => d.IdLoaiVeNavigation)
                    .WithMany(p => p.Ve)
                    .HasForeignKey(d => d.IdLoaiVe)
                    .HasConstraintName("FK__Ve__IdLoaiVe__5535A963");

                entity.HasOne(d => d.IdTaiKhoanNavigation)
                    .WithMany(p => p.Ve)
                    .HasForeignKey(d => d.IdTaiKhoan)
                    .HasConstraintName("FK__Ve__IdTaiKhoan__5441852A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

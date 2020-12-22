using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OnlineMoviesBooking.Models.Models
{
    public partial class CinemaContext : DbContext
    {
        public CinemaContext()
        {
        }

        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<Qa> Qa { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleClaim> RoleClaim { get; set; }
        public virtual DbSet<Screen> Screen { get; set; }
        public virtual DbSet<Seat> Seat { get; set; }
        public virtual DbSet<Show> Show { get; set; }
        public virtual DbSet<Theater> Theater { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TypeOfMember> TypeOfMember { get; set; }
        public virtual DbSet<TypesOfAccount> TypesOfAccount { get; set; }
        public virtual DbSet<TypesOfSeat> TypesOfSeat { get; set; }
        public virtual DbSet<UseDiscount> UseDiscount { get; set; }
        public virtual DbSet<VBillModel> VBillModel { get; set; }
        public virtual DbSet<VCheckout> VCheckout { get; set; }
        public virtual DbSet<VGetShowisComing> VGetShowisComing { get; set; }
        public virtual DbSet<VGetShowisUsed> VGetShowisUsed { get; set; }
        public virtual DbSet<VMovieComing> VMovieComing { get; set; }
        public virtual DbSet<VMovieNow> VMovieNow { get; set; }
        public virtual DbSet<VPrice> VPrice { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=Cinema;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Account__A9D10534644CB036")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdTypeOfMember)
                    .HasColumnName("Id_TypeOfMember")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdTypesOfUser)
                    .IsRequired()
                    .HasColumnName("Id_TypesOfUser")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasColumnName("SDT")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTypesOfUserNavigation)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.IdTypesOfUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_TypesOfAccount");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => new { e.IdSeat, e.IdAccount, e.IdShow });

                entity.Property(e => e.IdSeat)
                    .HasColumnName("Id_Seat")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdAccount)
                    .HasColumnName("Id_Account")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdShow)
                    .HasColumnName("Id_Show")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("('0')");

                entity.HasOne(d => d.CodeNavigation)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.Code)
                    .HasConstraintName("FK__Bill__Code__7FB5F314");

                entity.HasOne(d => d.IdAccountNavigation)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.IdAccount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bill_Account");

                entity.HasOne(d => d.IdSeatNavigation)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.IdSeat)
                    .HasConstraintName("FK_Bill_Seat");

                entity.HasOne(d => d.IdShowNavigation)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.IdShow)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bill_Show");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .HasName("U_Code")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.ImageDiscount)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Casts)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Director)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Genre)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Poster)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Rated).HasMaxLength(1000);

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");

                entity.Property(e => e.Trailer)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Qa>(entity =>
            {
                entity.ToTable("QA");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdAccount)
                    .IsRequired()
                    .HasColumnName("Id_Account")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.IdAccountNavigation)
                    .WithMany(p => p.Qa)
                    .HasForeignKey(d => d.IdAccount)
                    .HasConstraintName("FK_QaToAccount");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.HasKey(e => new { e.IdTypesOfAccount, e.IdRole });

                entity.Property(e => e.IdTypesOfAccount)
                    .HasColumnName("Id_TypesOfAccount")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdRole)
                    .HasColumnName("Id_Role")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.RoleClaim)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleClaim_Role");

                entity.HasOne(d => d.IdTypesOfAccountNavigation)
                    .WithMany(p => p.RoleClaim)
                    .HasForeignKey(d => d.IdTypesOfAccount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleClaim_TypesOfAccount");
            });

            modelBuilder.Entity<Screen>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdTheater)
                    .IsRequired()
                    .HasColumnName("Id_Theater")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdTheaterNavigation)
                    .WithMany(p => p.Screen)
                    .HasForeignKey(d => d.IdTheater)
                    .HasConstraintName("FK_Screen_Theater");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdScreen)
                    .IsRequired()
                    .HasColumnName("Id_Screen")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdTypesOfSeat)
                    .IsRequired()
                    .HasColumnName("Id_TypesOfSeat")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Row)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdScreenNavigation)
                    .WithMany(p => p.Seat)
                    .HasForeignKey(d => d.IdScreen)
                    .HasConstraintName("FK_Seat_Screen");

                entity.HasOne(d => d.IdTypesOfSeatNavigation)
                    .WithMany(p => p.Seat)
                    .HasForeignKey(d => d.IdTypesOfSeat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Seat_TypesOfSeat");
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdMovie)
                    .IsRequired()
                    .HasColumnName("Id_Movie")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdScreen)
                    .IsRequired()
                    .HasColumnName("Id_Screen")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Languages)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");

                entity.HasOne(d => d.IdMovieNavigation)
                    .WithMany(p => p.Show)
                    .HasForeignKey(d => d.IdMovie)
                    .HasConstraintName("FK_Show_Movie");

                entity.HasOne(d => d.IdScreenNavigation)
                    .WithMany(p => p.Show)
                    .HasForeignKey(d => d.IdScreen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Show_Screen");
            });

            modelBuilder.Entity<Theater>(entity =>
            {
                entity.HasIndex(e => e.Address)
                    .HasName("U_Id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Hotline)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.IdAccount)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdDiscount)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdSeat)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdShow)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdAccountNavigation)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.IdAccount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Account");

                entity.HasOne(d => d.IdDiscountNavigation)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.IdDiscount)
                    .HasConstraintName("FK_Ticket_Discount");

                entity.HasOne(d => d.IdSeatNavigation)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.IdSeat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Seat");

                entity.HasOne(d => d.IdShowNavigation)
                    .WithMany(p => p.Ticket)
                    .HasForeignKey(d => d.IdShow)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Show");
            });

            modelBuilder.Entity<TypeOfMember>(entity =>
            {
                entity.HasKey(e => e.IdTypeMember);

                entity.Property(e => e.IdTypeMember)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Content).HasMaxLength(1000);

                entity.Property(e => e.TypeOfMemberName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TypesOfAccount>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TypesOfSeat>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("U_TypesOfSeat")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UseDiscount>(entity =>
            {
                entity.HasKey(e => new { e.IdDiscount, e.IdAccount });

                entity.Property(e => e.IdDiscount)
                    .HasColumnName("Id_Discount")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdAccount)
                    .HasColumnName("Id_Account")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.HasOne(d => d.IdAccountNavigation)
                    .WithMany(p => p.UseDiscount)
                    .HasForeignKey(d => d.IdAccount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UseDiscount_Account");

                entity.HasOne(d => d.IdDiscountNavigation)
                    .WithMany(p => p.UseDiscount)
                    .HasForeignKey(d => d.IdDiscount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UseDiscount_Discount");
            });

            modelBuilder.Entity<VBillModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_BillModel");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.IdAccount)
                    .IsRequired()
                    .HasColumnName("Id_Account")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdShow)
                    .IsRequired()
                    .HasColumnName("Id_Show")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Languages)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.MovieName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ScreenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TheaterName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");
            });

            modelBuilder.Entity<VCheckout>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Checkout");

                entity.Property(e => e.IdMovie)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdShow)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Languages)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ScreenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TheaterName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TimeStart).HasColumnType("datetime");
            });

            modelBuilder.Entity<VGetShowisComing>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_GetShowisComing");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Languages)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.MovieName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Poster)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ScreenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TheaterName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");
            });

            modelBuilder.Entity<VGetShowisUsed>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_GetShowisUsed");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Languages)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.MovieName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Poster)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ScreenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TheaterName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");
            });

            modelBuilder.Entity<VMovieComing>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_MovieComing");

                entity.Property(e => e.Casts)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Director)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Genre)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Poster)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Rated).HasMaxLength(1000);

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");

                entity.Property(e => e.Trailer)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<VMovieNow>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_MovieNow");

                entity.Property(e => e.Casts)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Director)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Genre)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Poster)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Rated).HasMaxLength(1000);

                entity.Property(e => e.ReleaseDate).HasColumnType("datetime");

                entity.Property(e => e.Trailer)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<VPrice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_Price");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdAccount)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdDiscount)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdSeat)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdShow)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

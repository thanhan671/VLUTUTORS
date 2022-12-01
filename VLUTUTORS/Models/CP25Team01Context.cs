using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class CP25Team01Context : DbContext
    {
        public CP25Team01Context()
        {
        }

        public CP25Team01Context(DbContextOptions<CP25Team01Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Gioitinh> Gioitinhs { get; set; }
        public virtual DbSet<Lienhe> Lienhes { get; set; }
        public virtual DbSet<Mongiasu> Mongiasus { get; set; }
        public virtual DbSet<Taikhoannguoidung> Taikhoannguoidungs { get; set; }
        public virtual DbSet<Trangthai> Trangthais { get; set; }
        public virtual DbSet<Tuvan> Tuvans { get; set; }
        public virtual DbSet<Xetduyet> Xetduyets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tuleap.vanlanguni.edu.vn,18082;Database=CP25Team01;User Id=CP25Team01; Password=Cap25t01;Integrated Security=True;Trusted_Connection=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Gioitinh>(entity =>
            {
                entity.HasKey(e => e.IdgioiTinh);

                entity.ToTable("GIOITINH");

                entity.Property(e => e.IdgioiTinh).HasColumnName("IDGioiTinh");

                entity.Property(e => e.GioiTinh1)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("GioiTinh");
            });

            modelBuilder.Entity<Lienhe>(entity =>
            {
                entity.HasKey(e => e.IdlienHe);

                entity.ToTable("LIENHE");

                entity.Property(e => e.IdlienHe).HasColumnName("IDLienHe");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HoVaTen)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IdtrangThai).HasColumnName("IDTrangThai");

                entity.Property(e => e.MonHoc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NoiDung).IsRequired();

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("SDT")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Mongiasu>(entity =>
            {
                entity.HasKey(e => e.IdmonGiaSu);

                entity.ToTable("MONGIASU");

                entity.Property(e => e.IdmonGiaSu).HasColumnName("IDMonGiaSu");

                entity.Property(e => e.TenMonGiaSu)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Taikhoannguoidung>(entity =>
            {
                entity.ToTable("TAIKHOANNGUOIDUNG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnhChungChi).HasColumnType("image");

                entity.Property(e => e.AnhDaiDien).HasColumnType("image");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HoTen)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IdgioiTinh).HasColumnName("IDGioiTinh");

                entity.Property(e => e.IdmonGiaSu).HasColumnName("IDMonGiaSu");

                entity.Property(e => e.IdxetDuyet).HasColumnName("IDXetDuyet");

                entity.Property(e => e.KhoaDangHoc).HasMaxLength(100);

                entity.Property(e => e.MatKhau)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ThongTinTknganHang)
                    .HasMaxLength(200)
                    .HasColumnName("ThongTinTKNganHang");
            });

            modelBuilder.Entity<Trangthai>(entity =>
            {
                entity.HasKey(e => e.IdtrangThai)
                    .HasName("PK_TRANGTHAITUVAN");

                entity.ToTable("TRANGTHAI");

                entity.Property(e => e.IdtrangThai).HasColumnName("IDTrangThai");

                entity.Property(e => e.TrangThai1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("TrangThai");
            });

            modelBuilder.Entity<Tuvan>(entity =>
            {
                entity.HasKey(e => e.IdtuVan);

                entity.ToTable("TUVAN");

                entity.Property(e => e.IdtuVan).HasColumnName("IDTuVan");

                entity.Property(e => e.HoVaTen)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdtrangThai).HasColumnName("IDTrangThai");

                entity.Property(e => e.NoiDungTuVan).IsRequired();

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SDT");
            });

            modelBuilder.Entity<Xetduyet>(entity =>
            {
                entity.HasKey(e => e.IdxetDuyet);

                entity.ToTable("XETDUYET");

                entity.Property(e => e.IdxetDuyet).HasColumnName("IDXetDuyet");

                entity.Property(e => e.TenTrangThai)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

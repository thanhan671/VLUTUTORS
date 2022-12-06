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
        public virtual DbSet<Khoa> Khoas { get; set; }
        public virtual DbSet<Lienhe> Lienhes { get; set; }
        public virtual DbSet<Mongiasu> Mongiasus { get; set; }
        public virtual DbSet<Nganhang> Nganhangs { get; set; }
        public virtual DbSet<Noidung> Noidungs { get; set; }
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

            modelBuilder.Entity<Khoa>(entity =>
            {
                entity.HasKey(e => e.Idkhoa);

                entity.ToTable("KHOA");

                entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");

                entity.Property(e => e.TenKhoa)
                    .IsRequired()
                    .HasMaxLength(200);
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

                entity.HasOne(d => d.IdtrangThaiNavigation)
                    .WithMany(p => p.Lienhes)
                    .HasForeignKey(d => d.IdtrangThai)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LIENHE_TRANGTHAI");
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

            modelBuilder.Entity<Nganhang>(entity =>
            {
                entity.ToTable("NGANHANG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TenNganHangHoacViDienTu)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Noidung>(entity =>
            {
                entity.ToTable("NOIDUNG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DiaChi).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Facebook).IsRequired();

                entity.Property(e => e.GioiThieu).IsRequired();

                entity.Property(e => e.GioiThieuChanTrang).IsRequired();

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SDT");

                entity.Property(e => e.Slogan).IsRequired();
            });

            modelBuilder.Entity<Taikhoannguoidung>(entity =>
            {
                entity.ToTable("TAIKHOANNGUOIDUNG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnhDaiDien).HasColumnType("image");

                entity.Property(e => e.ChungChiMon1).IsUnicode(false);

                entity.Property(e => e.ChungChiMon2).IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HoTen)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IdgioiTinh).HasColumnName("IDGioiTinh");

                entity.Property(e => e.Idkhoa).HasColumnName("IDKhoa");

                entity.Property(e => e.IdmonGiaSu1).HasColumnName("IDMonGiaSu1");

                entity.Property(e => e.IdmonGiaSu2).HasColumnName("IDMonGiaSu2");

                entity.Property(e => e.IdnganHang).HasColumnName("IDNganHang");

                entity.Property(e => e.IdxetDuyet).HasColumnName("IDXetDuyet");

                entity.Property(e => e.MatKhau)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Sdt)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SoTaiKhoan).HasMaxLength(200);

                entity.HasOne(d => d.IdgioiTinhNavigation)
                    .WithMany(p => p.Taikhoannguoidungs)
                    .HasForeignKey(d => d.IdgioiTinh)
                    .HasConstraintName("FK_TAIKHOANNGUOIDUNG_GIOITINH");

                entity.HasOne(d => d.IdkhoaNavigation)
                    .WithMany(p => p.Taikhoannguoidungs)
                    .HasForeignKey(d => d.Idkhoa)
                    .HasConstraintName("FK_TAIKHOANNGUOIDUNG_KHOA");

                entity.HasOne(d => d.IdmonGiaSu1Navigation)
                    .WithMany(p => p.TaikhoannguoidungIdmonGiaSu1Navigations)
                    .HasForeignKey(d => d.IdmonGiaSu1)
                    .HasConstraintName("FK_TAIKHOANNGUOIDUNG_MONGIASU");

                entity.HasOne(d => d.IdmonGiaSu2Navigation)
                    .WithMany(p => p.TaikhoannguoidungIdmonGiaSu2Navigations)
                    .HasForeignKey(d => d.IdmonGiaSu2)
                    .HasConstraintName("FK_TAIKHOANNGUOIDUNG_MONGIASU1");

                entity.HasOne(d => d.IdnganHangNavigation)
                    .WithMany(p => p.Taikhoannguoidungs)
                    .HasForeignKey(d => d.IdnganHang)
                    .HasConstraintName("FK_TAIKHOANNGUOIDUNG_NGANHANG");

                entity.HasOne(d => d.IdxetDuyetNavigation)
                    .WithMany(p => p.Taikhoannguoidungs)
                    .HasForeignKey(d => d.IdxetDuyet)
                    .HasConstraintName("FK_TAIKHOANNGUOIDUNG_XETDUYET");
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

                entity.HasOne(d => d.IdtrangThaiNavigation)
                    .WithMany(p => p.Tuvans)
                    .HasForeignKey(d => d.IdtrangThai)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TUVAN_TRANGTHAI");
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

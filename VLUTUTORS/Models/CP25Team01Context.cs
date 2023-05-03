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

        public virtual DbSet<Baikiemtra> Baikiemtras { get; set; }
        public virtual DbSet<Caday> Cadays { get; set; }
        public virtual DbSet<Cahoc> Cahocs { get; set; }
        public virtual DbSet<Danhgiagiasu> Danhgiagiasus { get; set; }
        public virtual DbSet<Danhgianguoihoc> Danhgianguoihocs { get; set; }
        public virtual DbSet<Giasuyeuthich> Giasuyeuthichs { get; set; }
        public virtual DbSet<Gioitinh> Gioitinhs { get; set; }
        public virtual DbSet<Khoa> Khoas { get; set; }
        public virtual DbSet<Khoadaotao> Khoadaotaos { get; set; }
        public virtual DbSet<Lienhe> Lienhes { get; set; }
        public virtual DbSet<Loaituvan> Loaituvans { get; set; }
        public virtual DbSet<Mongiasu> Mongiasus { get; set; }
        public virtual DbSet<Naptien> Naptiens { get; set; }
        public virtual DbSet<Nganhang> Nganhangs { get; set; }
        public virtual DbSet<Noidung> Noidungs { get; set; }
        public virtual DbSet<Phiday> Phidays { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<Ruttien> Ruttiens { get; set; }
        public virtual DbSet<Taikhoanadmin> Taikhoanadmins { get; set; }
        public virtual DbSet<Taikhoannguoidung> Taikhoannguoidungs { get; set; }
        public virtual DbSet<Tieuchidanhgia> Tieuchidanhgias { get; set; }
        public virtual DbSet<Trangthai> Trangthais { get; set; }
        public virtual DbSet<Tuvan> Tuvans { get; set; }
        public virtual DbSet<Xetduyet> Xetduyets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tuleap.vanlanguni.edu.vn,18082;Database=CP25Team01;User Id=CP25Team01; Password=VLUTUTORS01;Integrated Security=True;Trusted_Connection=False;ApplicationIntent=ReadWrite;MultipleActiveResultSets=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Baikiemtra>(entity =>
            {
                entity.HasKey(e => e.IdCauHoi)
                    .HasName("PK_CAUHOI");

                entity.ToTable("BAIKIEMTRA");

                entity.Property(e => e.CauHoi)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DapAnA)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DapAnB)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DapAnC)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DapAnD)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DapAnDung)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Caday>(entity =>
            {
                entity.ToTable("CADAY");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdloaiCaDay).HasColumnName("IDLoaiCaDay");

                entity.Property(e => e.IdmonDay).HasColumnName("IDMonDay");

                entity.Property(e => e.IdnguoiDay).HasColumnName("IDNguoiDay");

                entity.Property(e => e.IdnguoiHoc).HasColumnName("IDNguoiHoc");

                entity.Property(e => e.Link)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NgayDay).HasColumnType("date");

                entity.HasOne(d => d.IdloaiCaDayNavigation)
                    .WithMany(p => p.Cadays)
                    .HasForeignKey(d => d.IdloaiCaDay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CADAY_CAHOC");

                entity.HasOne(d => d.IdmonDayNavigation)
                    .WithMany(p => p.Cadays)
                    .HasForeignKey(d => d.IdmonDay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CADAY_MONGIASU");

                entity.HasOne(d => d.IdnguoiDayNavigation)
                    .WithMany(p => p.Cadays)
                    .HasForeignKey(d => d.IdnguoiDay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CADAY_TAIKHOANNGUOIDUNG");
            });

            modelBuilder.Entity<Cahoc>(entity =>
            {
                entity.HasKey(e => e.IdCaHoc);

                entity.ToTable("CAHOC");
            });

            modelBuilder.Entity<Danhgiagiasu>(entity =>
            {
                entity.ToTable("DANHGIAGIASU");
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.NgayTao).HasColumnType("datetime");
            });

            modelBuilder.Entity<Danhgianguoihoc>(entity =>
            {
                entity.ToTable("DANHGIANGUOIHOC");
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.NgayTao).HasColumnType("datetime");
            });

            modelBuilder.Entity<Giasuyeuthich>(entity =>
            {
                entity.HasKey(e => new { e.GiasuId, e.NguoidungId });

                entity.ToTable("GIASUYEUTHICH");
            });

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

            modelBuilder.Entity<Khoadaotao>(entity =>
            {
                entity.HasKey(e => e.IdBaiHoc);

                entity.ToTable("KHOADAOTAO");

                entity.Property(e => e.TenBaiHoc)
                    .IsRequired()
                    .HasMaxLength(500);
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

            modelBuilder.Entity<Loaituvan>(entity =>
            {
                entity.HasKey(e => e.IdLoaiTuVan);

                entity.ToTable("LOAITUVAN");

                entity.Property(e => e.TenLoaiTuVan)
                    .IsRequired()
                    .HasMaxLength(300);
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

            modelBuilder.Entity<Naptien>(entity =>
            {
                entity.ToTable("NAPTIEN");

                entity.Property(e => e.MaNapTien)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ThoiGianNapTien).HasColumnType("smalldatetime");

                entity.HasOne(d => d.IdNguoiNapNavigation)
                    .WithMany(p => p.Naptiens)
                    .HasForeignKey(d => d.IdNguoiNap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NAPTIEN_TAIKHOANNGUOIDUNG");
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

                entity.Property(e => e.AnhGioiThieu)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DiaChi).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Facebook).IsRequired();

                entity.Property(e => e.GioiThieu).IsRequired();

                entity.Property(e => e.GioiThieu1).IsRequired();

                entity.Property(e => e.GioiThieu2).IsRequired();

                entity.Property(e => e.GioiThieu3).IsRequired();

                entity.Property(e => e.GioiThieuChanTrang).IsRequired();

                entity.Property(e => e.Sdt)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("SDT");

                entity.Property(e => e.Slogan).IsRequired();

                entity.Property(e => e.TieuDeGt1)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("TieuDeGT1");

                entity.Property(e => e.TieuDeGt2)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("TieuDeGT2");

                entity.Property(e => e.TieuDeGt3)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("TieuDeGT3");
            });

            modelBuilder.Entity<Phiday>(entity =>
            {
                entity.ToTable("PHIDAY");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Quyen>(entity =>
            {
                entity.HasKey(e => e.IdQuyen);

                entity.ToTable("QUYEN");

                entity.Property(e => e.TenQuyen)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Ruttien>(entity =>
            {
                entity.ToTable("RUTTIEN");

                entity.Property(e => e.MaRutTien)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ThoiGianRutTien).HasColumnType("smalldatetime");

                entity.HasOne(d => d.IdNguoiRutNavigation)
                    .WithMany(p => p.Ruttiens)
                    .HasForeignKey(d => d.IdNguoiRut)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RUTTIEN_TAIKHOANNGUOIDUNG");
            });

            modelBuilder.Entity<Taikhoanadmin>(entity =>
            {
                entity.ToTable("TAIKHOANADMIN");

                entity.Property(e => e.MatKhau)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TaiKhoan)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdQuyenNavigation)
                    .WithMany(p => p.Taikhoanadmins)
                    .HasForeignKey(d => d.IdQuyen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TAIKHOANADMIN_QUYEN");
            });

            modelBuilder.Entity<Taikhoannguoidung>(entity =>
            {
                entity.ToTable("TAIKHOANNGUOIDUNG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnhDaiDien).IsUnicode(false);

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

                entity.Property(e => e.NgayTao).HasColumnType("date");

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

            modelBuilder.Entity<Tieuchidanhgia>(entity =>
            {
                entity.HasKey(e => e.IdTieuChi);

                entity.ToTable("TIEUCHIDANHGIA");

                entity.Property(e => e.IdTieuChi).ValueGeneratedNever();

                entity.Property(e => e.DanhCho).HasMaxLength(50);

                entity.Property(e => e.TieuChi)
                    .IsRequired()
                    .HasMaxLength(100);
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

                entity.Property(e => e.Email).HasMaxLength(255);

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

                entity.HasOne(d => d.IdLoaiTuVanNavigation)
                    .WithMany(p => p.Tuvans)
                    .HasForeignKey(d => d.IdLoaiTuVan)
                    .HasConstraintName("FK_TUVAN_LOAITUVAN");

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

                entity.Property(e => e.IdxetDuyet)
                    .ValueGeneratedNever()
                    .HasColumnName("IDXetDuyet");

                entity.Property(e => e.TenTrangThai).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

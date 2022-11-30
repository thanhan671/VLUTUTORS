using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Taikhoannguoidung
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public int? IdgioiTinh { get; set; }
        public string Sdt { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string KhoaDangHoc { get; set; }
        public byte[] AnhDaiDien { get; set; }
        public bool? TrangThaiTaiKhoan { get; set; }
        public string ThongTinTknganHang { get; set; }
        public string GioiThieu { get; set; }
        public int? IdmonGiaSu { get; set; }
        public string DanhGiaVeViecGiaSu { get; set; }
        public string GioiThieuVeMonGiaSu { get; set; }
        public double? DiemTrungBinh { get; set; }
        public string TenChungChi { get; set; }
        public byte[] AnhChungChi { get; set; }
        public int? IdxetDuyet { get; set; }
    }
}

using System;

namespace VLUTUTORS.Models
{
    public class Danhgianguoihoc
    {
        public int Id { get; set; }
        public int IdCaDay { get; set; }
        public string TieuChi { get; set; }
        public string DanhGia { get; set; }
        public int Diem { get; set; }
        public int IdGiaSu { get; set; }
        public int IdNguoiDung { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}

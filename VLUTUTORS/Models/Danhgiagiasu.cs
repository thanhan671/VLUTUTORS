using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VLUTUTORS.Models
{
    public class Danhgiagiasu
    {
        public int Id { get; set; }
        public int IdCaDay { get; set; }
        public string TieuChi { get; set; }
        public string DanhGia { get; set; }
        public int Diem { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public int GiasuId { get; set; }
        public int NguoidungId { get; set; }

        [NotMapped]
        public string tenNguoiDay { get; set; }
        [NotMapped]
        public string tenNguoiHoc { get; set; }

    }
}
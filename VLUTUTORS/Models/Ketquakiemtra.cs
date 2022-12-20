using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Ketquakiemtra
    {
        public int Id { get; set; }
        public int IdBaiKiemTra { get; set; }
        public int IdNguoiLam { get; set; }
        public string DapAnDaChon { get; set; }
        public double? KetQua { get; set; }
    }
}

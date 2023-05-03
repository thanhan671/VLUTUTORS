using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Phiday
    {
        public int Id { get; set; }
        public int? ChietKhau { get; set; }
        public string SoTaiKhoan { get; set; }
        public string NganHang { get; set; }
        public string NguoiNhan { get; set; }
    }
}

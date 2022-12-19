using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Taikhoanadmin
    {
        public int Id { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public int IdQuyen { get; set; }

        public virtual Quyen IdQuyenNavigation { get; set; }
    }
}

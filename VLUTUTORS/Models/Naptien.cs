using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Naptien
    {
        public int Id { get; set; }
        public string MaNapTien { get; set; }
        public int IdNguoiNap { get; set; }
        public int SoTienNap { get; set; }
        public bool TrangThai { get; set; }

        public virtual Taikhoannguoidung IdNguoiNapNavigation { get; set; }
    }
}

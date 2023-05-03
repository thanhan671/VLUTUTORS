using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime ThoiGianNapTien { get; set; }

        [NotMapped]
        public string tenNguoiNap { get; set; }

        [NotMapped]
        public string trangThai { get; set; }

        public virtual Taikhoannguoidung IdNguoiNapNavigation { get; set; }
    }
}
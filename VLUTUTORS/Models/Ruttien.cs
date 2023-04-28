using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Ruttien
    {
        public int Id { get; set; }
        public string MaRutTien { get; set; }
        public int IdNguoiRut { get; set; }
        public int SoTienRut { get; set; }
        public bool TrangThai { get; set; }

        public virtual Taikhoannguoidung IdNguoiRutNavigation { get; set; }
    }
}

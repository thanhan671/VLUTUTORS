using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Xetduyet
    {
        public Xetduyet()
        {
            Taikhoannguoidungs = new HashSet<Taikhoannguoidung>();
        }

        public int IdxetDuyet { get; set; }
        public string TenTrangThai { get; set; }

        public virtual ICollection<Taikhoannguoidung> Taikhoannguoidungs { get; set; }
    }
}

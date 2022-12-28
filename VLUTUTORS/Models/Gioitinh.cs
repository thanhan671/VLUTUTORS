using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Gioitinh
    {
        public Gioitinh()
        {
            Taikhoannguoidungs = new HashSet<Taikhoannguoidung>();
        }

        public int IdgioiTinh { get; set; }
        public string GioiTinh1 { get; set; }

        public virtual ICollection<Taikhoannguoidung> Taikhoannguoidungs { get; set; }
    }
}

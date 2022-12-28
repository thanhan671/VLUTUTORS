using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Nganhang
    {
        public Nganhang()
        {
            Taikhoannguoidungs = new HashSet<Taikhoannguoidung>();
        }

        public int Id { get; set; }
        public string TenNganHangHoacViDienTu { get; set; }

        public virtual ICollection<Taikhoannguoidung> Taikhoannguoidungs { get; set; }
    }
}

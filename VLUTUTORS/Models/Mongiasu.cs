using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Mongiasu
    {
        public Mongiasu()
        {
            Khoadaotaos = new HashSet<Khoadaotao>();
            TaikhoannguoidungIdmonGiaSu1Navigations = new HashSet<Taikhoannguoidung>();
            TaikhoannguoidungIdmonGiaSu2Navigations = new HashSet<Taikhoannguoidung>();
        }

        public int IdmonGiaSu { get; set; }
        public string TenMonGiaSu { get; set; }

        public virtual ICollection<Khoadaotao> Khoadaotaos { get; set; }
        public virtual ICollection<Taikhoannguoidung> TaikhoannguoidungIdmonGiaSu1Navigations { get; set; }
        public virtual ICollection<Taikhoannguoidung> TaikhoannguoidungIdmonGiaSu2Navigations { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Khoadaotao
    {
        public Khoadaotao()
        {
            Baihocs = new HashSet<Baihoc>();
            Baikiemtras = new HashSet<Baikiemtra>();
        }

        public int IdKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; }

        public virtual ICollection<Baihoc> Baihocs { get; set; }
        public virtual ICollection<Baikiemtra> Baikiemtras { get; set; }
    }
}

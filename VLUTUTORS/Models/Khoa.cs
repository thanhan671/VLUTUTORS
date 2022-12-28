using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Khoa
    {
        public Khoa()
        {
            Taikhoannguoidungs = new HashSet<Taikhoannguoidung>();
        }

        public int Idkhoa { get; set; }
        public string TenKhoa { get; set; }

        public virtual ICollection<Taikhoannguoidung> Taikhoannguoidungs { get; set; }
    }
}

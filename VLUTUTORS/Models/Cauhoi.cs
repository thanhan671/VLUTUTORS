using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Cauhoi
    {
        public int IdCauHoi { get; set; }
        public string CauHoi1 { get; set; }
        public string DapAnA { get; set; }
        public string DapAnB { get; set; }
        public string DapAnC { get; set; }
        public string DapAnD { get; set; }
        public string DapAnDung { get; set; }
        public int IdBaiKiemTra { get; set; }

        public virtual Baikiemtra IdBaiKiemTraNavigation { get; set; }
    }
}

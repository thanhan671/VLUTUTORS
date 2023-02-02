using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Chitietbaihoc
    {
        public int IdNoiDung { get; set; }
        public string TenBaiHoc { get; set; }
        public string LinkVideo { get; set; }
        public string LinkTaiLieu { get; set; }
        public int IdKhoaDaoTao { get; set; }

        public virtual Khoadaotao IdKhoaDaoTaoNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Baihoc
    {
        public int IdBaiHoc { get; set; }
        public string TenBaiHoc { get; set; }
        public string LinkBaiHoc { get; set; }
        public int IdKhoaHoc { get; set; }

        public virtual Khoadaotao IdKhoaHocNavigation { get; set; }
    }
}

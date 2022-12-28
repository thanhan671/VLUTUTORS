using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        public Baihoc()
        {
        }
        public Baihoc(int IdBaiHoc, string TenBaiHoc, string LinkBaiHoc)
        {
            this.IdBaiHoc = IdBaiHoc;
            this.TenBaiHoc = TenBaiHoc;
            this.LinkBaiHoc = LinkBaiHoc;
        }

        [NotMapped]
        public List<string> courses { get; set; }
    }
}

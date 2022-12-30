using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Khoadaotao
    {
        public int IdBaiHoc { get; set; }
        public string TenBaiHoc { get; set; }
        public string Link { get; set; }

        public Khoadaotao()
        {
        }
        public Khoadaotao(int IdBaiHoc, string TenBaiHoc, string Link)
        {
            this.IdBaiHoc = IdBaiHoc;
            this.TenBaiHoc = TenBaiHoc;
            this.Link = Link;
        }

        [NotMapped]
        public List<string> courses { get; set; }
        public string TaiLieu { get; set; }
    }
}

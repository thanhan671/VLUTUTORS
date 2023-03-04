using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Khoadaotao
    {
        public int IdBaiHoc { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        public string TenBaiHoc { get; set; }
        public string TaiLieu { get; set; }
        public string LinkVideo { get; set; }

        public Khoadaotao(int IdBaiHoc, string TenBaiHoc, string Link, string TaiLieu)
        {
            this.IdBaiHoc = IdBaiHoc;
            this.TenBaiHoc = TenBaiHoc;
            this.LinkVideo = Link;
            this.TaiLieu = TaiLieu;
        }

        public Khoadaotao()
        {
        }

        [NotMapped]
        public List<string> courses { get; set; }

        [NotMapped]
        public List<string> courseLink { get; set; }

        [NotMapped]
        public double? currentScore { get; set; }
    }
}
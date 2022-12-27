using Microsoft.AspNetCore.Mvc.Rendering;
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

        [NotMapped]
        public string KhoaHoc { get; set; }
        [NotMapped]
        public SelectList KhoaHocs { get; set; }

        public virtual Khoadaotao IdKhoaHocNavigation { get; set; }
    }
}

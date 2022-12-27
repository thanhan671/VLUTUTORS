using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Baikiemtra
    {
        public int IdBaiKiemTra { get; set; }
        public string CauHoi { get; set; }
        public string DapAnA { get; set; }
        public string DapAnB { get; set; }
        public string DapAnc { get; set; }
        public string DapAnD { get; set; }
        public string DapAnDung { get; set; }
        public int IdKhoaHoc { get; set; }

        [NotMapped]
        public string KhoaHoc { get; set; }
        [NotMapped]
        public SelectList KhoaHocs { get; set; }
        public virtual Khoadaotao IdKhoaHocNavigation { get; set; }
    }
}

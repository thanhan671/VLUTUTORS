using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models 
{
    public partial class Caday 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int IdnguoiDay { get; set; }

        public int IdmonDay { get; set; }

        public int IdloaiCaDay { get; set; }
        public DateTime NgayDay { get; set; }
        public int GioBatDau { get; set; }
        public int PhutBatDau { get; set; }
        public int GioKetThuc { get; set; }
        public int PhutKetThuc { get; set; }
        public bool? LapLich { get; set; }
        public int? IdnguoiHoc { get; set; }
        public string Link { get; set; }
        public bool? TrangThai { get; set; }

        [NotMapped]
        public string tenNguoiDay { get; set; }
        [NotMapped]
        public string tenNguoiHoc { get; set; }
        [NotMapped]
        public string tenMonDay { get; set; }
        [NotMapped]
        public string tenLoaiCaDay { get; set; }
        [NotMapped]
        public double giaCaDay { get; set; }

        [NotMapped]
        public SelectList subjectItems { get; set; }

        [NotMapped]
        public SelectList teachTimeItems { get; set; }

        public virtual Cahoc IdloaiCaDayNavigation { get; set; }
        public virtual Mongiasu IdmonDayNavigation { get; set; }
        public virtual Taikhoannguoidung IdnguoiDayNavigation { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using VLUTUTORS.Models;

namespace VLUTUTORS.Areas.Tutors.Models
{
    public class LessonPlan
    {
        public int ID;
        public int IDNguoiDay;
        public int IDMonDay;
        public int IDLoaiCaDay;
        public DateTime NgayDay;
        public int GioBatDau;
        public int PhutBatDau;
        public int GioKetThuc;
        public int PhutKetThuc;
        public bool lapLich;

        [NotMapped]
        public SelectList Subjects { get; set; }

        [NotMapped]
        public SelectList HourPlan { get; set; }

        public virtual Taikhoannguoidung IDNguoiDungNavigation { get; set; }
        public virtual Mongiasu IDMonGiaSuNavigation { get; set; }
        public virtual LoaiCaDay IdCaDayNavigation { get; set; }
    }
}

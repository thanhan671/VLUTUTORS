using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Baikiemtra
    {
        public Baikiemtra()
        {
            Cauhois = new HashSet<Cauhoi>();
        }

        public int IdBaiKiemTra { get; set; }
        public int IdKhoaDaoTao { get; set; }

        [NotMapped]
        public string KhoaHoc { get; set; }
        [NotMapped]
        public SelectList KhoaHocs { get; set; }

        public virtual Khoadaotao IdKhoaDaoTaoNavigation { get; set; }
        public virtual ICollection<Cauhoi> Cauhois { get; set; }
    }
}

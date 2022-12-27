using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Khoadaotao
    {
        public Khoadaotao()
        {
            Baihocs = new HashSet<Baihoc>();
            Baikiemtras = new HashSet<Baikiemtra>();
        }

        public int IdKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; }
        public int IdMonGiaSu { get; set; }

        [NotMapped]
        public string MonGiaSu { get; set; }
        [NotMapped]
        public SelectList MonGiaSus { get; set; }
        public virtual Mongiasu IdMonGiaSuNavigation { get; set; }
        public virtual ICollection<Baihoc> Baihocs { get; set; }
        public virtual ICollection<Baikiemtra> Baikiemtras { get; set; }
    }
}

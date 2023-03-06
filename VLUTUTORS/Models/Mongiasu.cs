using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Mongiasu
    {
        public Mongiasu()
        {
            Cadays = new HashSet<Caday>();
            TaikhoannguoidungIdmonGiaSu1Navigations = new HashSet<Taikhoannguoidung>();
            TaikhoannguoidungIdmonGiaSu2Navigations = new HashSet<Taikhoannguoidung>();
        }

        public int IdmonGiaSu { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        public string TenMonGiaSu { get; set; }

        public virtual ICollection<Caday> Cadays { get; set; }
        public virtual ICollection<Taikhoannguoidung> TaikhoannguoidungIdmonGiaSu1Navigations { get; set; }
        public virtual ICollection<Taikhoannguoidung> TaikhoannguoidungIdmonGiaSu2Navigations { get; set; }
    }
}
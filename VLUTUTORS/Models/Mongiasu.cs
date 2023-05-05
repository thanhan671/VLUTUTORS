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
        [RegularExpression(@"[A-Za-z 0-9]", ErrorMessage = "Không nhập ký tự đặc biệt cho trường này!")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Giới hạn từ 2-50 ký tự")]
        public string TenMonGiaSu { get; set; }

        public virtual ICollection<Caday> Cadays { get; set; }
        public virtual ICollection<Taikhoannguoidung> TaikhoannguoidungIdmonGiaSu1Navigations { get; set; }
        public virtual ICollection<Taikhoannguoidung> TaikhoannguoidungIdmonGiaSu2Navigations { get; set; }
    }
}
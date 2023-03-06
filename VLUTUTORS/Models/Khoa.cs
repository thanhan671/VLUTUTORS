using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Khoa
    {
        public Khoa()
        {
            Taikhoannguoidungs = new HashSet<Taikhoannguoidung>();
        }

        public int Idkhoa { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        public string TenKhoa { get; set; }

        public virtual ICollection<Taikhoannguoidung> Taikhoannguoidungs { get; set; }
    }
}
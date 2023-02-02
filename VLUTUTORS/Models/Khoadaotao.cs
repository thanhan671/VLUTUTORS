using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Khoadaotao
    {
        public Khoadaotao()
        {
            Chitietbaihocs = new HashSet<Chitietbaihoc>();
        }

        public int IdBaiHoc { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string TenBaiHoc { get; set; }

        public virtual ICollection<Chitietbaihoc> Chitietbaihocs { get; set; }
    }
}

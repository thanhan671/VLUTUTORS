using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Cahoc
    {
        public int IdCaHoc { get; set; }

        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public int LoaiCa { get; set; }

        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public double GiaTien { get; set; }
    }
}

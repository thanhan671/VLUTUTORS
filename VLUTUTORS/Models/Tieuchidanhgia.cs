using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Tieuchidanhgia
    {
        public int IdTieuChi { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        [StringLength(maximumLength: 30, ErrorMessage = "Tối đa 30 ký tự")]
        public string TieuChi { get; set; }

        public string DanhCho { get; set; }
    }
}

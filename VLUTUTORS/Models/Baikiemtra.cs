using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Baikiemtra
    {

        public int IdCauHoi { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string CauHoi { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string DapAnA { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string DapAnB { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string DapAnC { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string DapAnD { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string DapAnDung { get; set; }
    }
}

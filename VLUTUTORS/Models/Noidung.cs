using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Noidung
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string GioiThieuChanTrang { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string Sdt { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string Facebook { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string GioiThieu { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string Slogan { get; set; }
    }
}
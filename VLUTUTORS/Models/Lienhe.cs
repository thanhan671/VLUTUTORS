using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Lienhe
    {
        public int IdlienHe { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string HoVaTen { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string MonHoc { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string Sdt { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string NoiDung { get; set; }

        public int IdtrangThai { get; set; }

        [NotMapped]
        public string TrangThai { get; set; }
        [NotMapped]
        public SelectList TrangThais { get; set; }

        public virtual Trangthai IdtrangThaiNavigation { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Taikhoanadmin
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string TaiKhoan { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public int IdQuyen { get; set; }


        [NotMapped]
        public string Quyen { get; set; }
        [NotMapped]
        public SelectList Quyens { get; set; }
        public virtual Quyen IdQuyenNavigation { get; set; }
    }
}
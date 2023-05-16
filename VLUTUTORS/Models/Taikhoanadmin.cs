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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Giới hạn từ 5-30 ký tự")]
        public string TaiKhoan { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải trên 6 ký tự")]
        public string MatKhau { get; set; }

        public int IdQuyen { get; set; }


        [NotMapped]
        public string Quyen { get; set; }
        [NotMapped]
        public SelectList listQuyen { get; set; }
        public virtual Quyen IdQuyenNavigation { get; set; }
    }
}
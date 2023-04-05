using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Tuvan
    {
        public int IdtuVan { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này! Giới hạn từ 5-25 ký tự")]
        [StringLength(maximumLength: 25, MinimumLength = 5, ErrorMessage = "Giới hạn từ 5-25 ký tự")]
        public string HoVaTen { get; set; }

        [StringLength(maximumLength: 64, MinimumLength = 18, ErrorMessage = "Giới hạn từ 18-64 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này! Giới hạn từ 18-64 ký tự")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ. Định dạng email chính xác là example@gmail.com.")]
        public string Email { get; set; }

        [StringLength(maximumLength: 11, MinimumLength = 10, ErrorMessage = "Giới hạn từ 10-11 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        public string Sdt { get; set; }

        [StringLength(maximumLength: 1000, MinimumLength = 50, ErrorMessage = "Giới hạn từ 50-1000 ký tự")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này! Giới hạn từ 50-1000 ký tự")]
        public string NoiDungTuVan { get; set; }
        public int IdtrangThai { get; set; }
        public int? IdLoaiTuVan { get; set; }

        public virtual Trangthai IdtrangThaiNavigation { get; set; }

        [NotMapped]
        public string TrangThai { get; set; }
        [NotMapped]
        public SelectList TrangThais { get; set; }

        [NotMapped]
        public SelectList LoaiTuVan { get; set; }

        public virtual Loaituvan IdLoaiTuVanNavigation { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Taikhoannguoidung
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải trên 6 ký tự")]
        public string MatKhau { get; set; }
        public int? IdgioiTinh { get; set; }
        public string Sdt { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? Idkhoa { get; set; }
        public byte[] AnhDaiDien { get; set; }
        public bool? TrangThaiTaiKhoan { get; set; }
        public string SoTaiKhoan { get; set; }
        public int? IdnganHang { get; set; }
        public string GioiThieu { get; set; }
        public string DanhGiaVeViecGiaSu { get; set; }
        public double? DiemTrungBinh { get; set; }
        public int? IdmonGiaSu1 { get; set; }
        public string ChungChiMon1 { get; set; }
        public string GioiThieuVeMonGiaSu1 { get; set; }
        public int? IdmonGiaSu2 { get; set; }
        public string ChungChiMon2 { get; set; }
        public string GioiThieuVeMonGiaSu2 { get; set; }
        public int? IdxetDuyet { get; set; }

        public virtual Gioitinh IdgioiTinhNavigation { get; set; }
        public virtual Khoa IdkhoaNavigation { get; set; }
        public virtual Mongiasu IdmonGiaSu1Navigation { get; set; }
        public virtual Mongiasu IdmonGiaSu2Navigation { get; set; }
        public virtual Nganhang IdnganHangNavigation { get; set; }
        public virtual Xetduyet IdxetDuyetNavigation { get; set; }

        [NotMapped]
        public List<SelectListItem> KhoaItems { get; set; }

    }
}

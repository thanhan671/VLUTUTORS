using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Taikhoannguoidung
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string HoTen { get; set; }

        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Không hợp lệ")]
        public string Email { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email không đúng định dạng")]
        [Remote(action: "Register", controller: "Accounts")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải trên 6 ký tự")]
        public string MatKhau { get; set; } = null!;
        public int IdgioiTinh { get; set; }
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
        public string TenChungChiHoacDiemMon1 { get; set; }
        public string TenChungChiHoacDiemMon2 { get; set; }

        public virtual Gioitinh IdgioiTinhNavigation { get; set; }
        public virtual Khoa IdkhoaNavigation { get; set; }
        public virtual Mongiasu IdmonGiaSu1Navigation { get; set; }
        public virtual Mongiasu IdmonGiaSu2Navigation { get; set; }
        public virtual Nganhang IdnganHangNavigation { get; set; }
        public virtual Xetduyet IdxetDuyetNavigation { get; set; }

        [NotMapped]
        public SelectList DepartmentItems { get; set; }

        [NotMapped]
        //public List<SelectListItem> GenderItems { get; set; }
        public SelectList GenderItems { get; set; }

        [NotMapped]
        public SelectList BankItems { get; set; }

        [NotMapped]
        public SelectList Subject1Items { get; set; }

        [NotMapped]
        public SelectList Subject2Items { get; set; }


        [NotMapped]
        public string GioiTinh { get; set; }
        [NotMapped]
        public SelectList GioiTinhs { get; set; }

        public virtual Gioitinh IdgioiTinhNavigation { get; set; }
        public virtual Khoa IdkhoaNavigation { get; set; }
        public virtual Mongiasu IdmonGiaSu1Navigation { get; set; }
        public virtual Mongiasu IdmonGiaSu2Navigation { get; set; }
        public virtual Nganhang IdnganHangNavigation { get; set; }
        public virtual Xetduyet IdxetDuyetNavigation { get; set; }

        //public SelectList Khoas { get; set; }

    }
}
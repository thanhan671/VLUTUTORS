using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Taikhoannguoidung : IValidatableObject
    {
        public Taikhoannguoidung()
        {
            CadayIdnguoiDayNavigations = new HashSet<Caday>();
            CadayIdnguoiHocNavigations = new HashSet<Caday>();
        }

        public int Id { get; set; }

        public string HoTen { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Không hợp lệ")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải trên 6 ký tự")]
        public string MatKhau { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn trường này")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Vui lòng chọn trường này")]
        public int? IdgioiTinh { get; set; }
        public string Sdt { get; set; }
        public DateTime? NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trường này")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Vui lòng chọn trường này")]
        public int? Idkhoa { get; set; }
        public string AnhDaiDien { get; set; }
        public bool? TrangThaiTaiKhoan { get; set; }
        public string SoTaiKhoan { get; set; }
        public int? IdnganHang { get; set; }

        //[RegularExpression("^([a-zA-Z0-9 .,:'-]+)$", ErrorMessage = "Không nhập ký tự đặc biệt")]
        public string GioiThieu { get; set; }

        //[RegularExpression("^([a-zA-Z0-9 .,:'-]+)$", ErrorMessage = "Không nhập ký tự đặc biệt")]
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
        public double? DiemBaiTest { get; set; }
        public bool? TrangThaiGiaSu { get; set; }
        public int? MaXacThuc { get; set; }
        public bool? XacThuc { get; set; }

        public virtual Gioitinh IdgioiTinhNavigation { get; set; }
        public virtual Khoa IdkhoaNavigation { get; set; }
        public virtual Mongiasu IdmonGiaSu1Navigation { get; set; }
        public virtual Mongiasu IdmonGiaSu2Navigation { get; set; }
        public virtual Nganhang IdnganHangNavigation { get; set; }
        public virtual Xetduyet IdxetDuyetNavigation { get; set; }
        public virtual ICollection<Caday> Cadays { get; set; }
        [NotMapped]
        public SelectList DepartmentItems { get; set; }

        [NotMapped]
        public SelectList GenderItems { get; set; }

        [NotMapped]
        public SelectList BankItems { get; set; }

        [NotMapped]
        public SelectList Subject1Items { get; set; }

        [NotMapped]
        public SelectList Subject2Items { get; set; }

        [NotMapped]
        public Microsoft.AspNetCore.Http.IFormFile avatarImage { get; set; }

        public virtual ICollection<Caday> CadayIdnguoiDayNavigations { get; set; }
        public virtual ICollection<Caday> CadayIdnguoiHocNavigations { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IdmonGiaSu1.HasValue && IdmonGiaSu2.HasValue)
            {
                if (IdmonGiaSu1 == IdmonGiaSu2)
                    yield return new ValidationResult(
                        "Phải chọn hai môn khác nhau", new[] { "IdmonGiaSu2" });
            }
            else
            if (IdmonGiaSu2.HasValue)
            {
                if (!string.IsNullOrEmpty(TenChungChiHoacDiemMon2))
                    yield return new ValidationResult(
                        "Vui lòng nhập Điểm trung bình môn hoặc tên chứng chỉ", new[] { "TenChungChiHoacDiemMon2" });
                if (!string.IsNullOrEmpty(ChungChiMon2))
                    yield return new ValidationResult(
                        "Vui lòng nhập Minh chứng kèm theo", new[] { "ChungChiMon2" });
                if (!string.IsNullOrEmpty(GioiThieuVeMonGiaSu2))
                    yield return new ValidationResult(
                        "Giới thiệu môn gia sư", new[] { "GioiThieuVeMonGiaSu2" });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Quyen
    {
        public Quyen()
        {
            Taikhoanadmins = new HashSet<Taikhoanadmin>();
        }

        public int IdQuyen { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        [RegularExpression(@"[A-Za-z 0-9]", ErrorMessage = "Không nhập ký tự đặc biệt cho trường này!")]
        [StringLength(maximumLength: 40, ErrorMessage = "Tối đa 40 ký tự")]
        public string TenQuyen { get; set; }

        public virtual ICollection<Taikhoanadmin> Taikhoanadmins { get; set; }
    }
}
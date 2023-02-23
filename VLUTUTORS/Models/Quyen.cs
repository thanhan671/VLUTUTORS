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
        public string TenQuyen { get; set; }

        public virtual ICollection<Taikhoanadmin> Taikhoanadmins { get; set; }
    }
}

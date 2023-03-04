using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Loaituvan
    {
        public Loaituvan()
        {
            Tuvans = new HashSet<Tuvan>();
        }

        public int IdLoaiTuVan { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        public string TenLoaiTuVan { get; set; }

        public virtual ICollection<Tuvan> Tuvans { get; set; }
    }
}
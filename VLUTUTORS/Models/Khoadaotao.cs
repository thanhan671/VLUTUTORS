using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Khoadaotao
    {
        public Khoadaotao()
        {
            Chitietbaihocs = new HashSet<Chitietbaihoc>();
        }

        public int IdBaiHoc { get; set; }
        [Required(ErrorMessage = "Vui lòng điền trường này!")]
        public string TenBaiHoc { get; set; }

        public virtual ICollection<Chitietbaihoc> Chitietbaihocs { get; set; }

        [NotMapped]
        public List<string> courses { get; set; }

        [NotMapped]
        public double? currentScore { get; set; }
    }
}

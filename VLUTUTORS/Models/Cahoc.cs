using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Cahoc
    {
        public int IdCaHoc { get; set; }
        public Cahoc()
        {
            Cadays = new HashSet<Caday>();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        public int LoaiCa { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền trường này!")]
        public double GiaTien { get; set; }

        public virtual ICollection<Caday> Cadays { get; set; }
    }
}

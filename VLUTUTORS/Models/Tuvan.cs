using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Tuvan
    {
        public int IdtuVan { get; set; }
        public string HoVaTen { get; set; }
        public string Sdt { get; set; }
        public string NoiDungTuVan { get; set; }
        public int IdtrangThai { get; set; }

        public virtual Trangthai IdtrangThaiNavigation { get; set; }

        [NotMapped]
        public string TrangThai { get; set; }
        [NotMapped]
        public SelectList TrangThais { get; set; }
    }
}

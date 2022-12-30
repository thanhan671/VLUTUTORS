using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Lienhe
    {
        public int IdlienHe { get; set; }
        public string HoVaTen { get; set; }
        public string Email { get; set; }
        public string MonHoc { get; set; }
        public string Sdt { get; set; }
        public string NoiDung { get; set; }
        public int IdtrangThai { get; set; }

        [NotMapped]
        public string TrangThai { get; set; }
        [NotMapped]
        public SelectList TrangThais { get; set; }

        public virtual Trangthai IdtrangThaiNavigation { get; set; }
    }
}

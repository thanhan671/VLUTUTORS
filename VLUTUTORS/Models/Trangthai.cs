using System;
using System.Collections.Generic;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Trangthai
    {
        public Trangthai()
        {
            Lienhes = new HashSet<Lienhe>();
            Tuvans = new HashSet<Tuvan>();
        }

        public int IdtrangThai { get; set; }
        public string TrangThai1 { get; set; }

        public virtual ICollection<Lienhe> Lienhes { get; set; }
        public virtual ICollection<Tuvan> Tuvans { get; set; }
    }
}

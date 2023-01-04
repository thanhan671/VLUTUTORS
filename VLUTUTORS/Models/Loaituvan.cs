using System;
using System.Collections.Generic;

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
        public string TenLoaiTuVan { get; set; }

        public virtual ICollection<Tuvan> Tuvans { get; set; }
    }
}

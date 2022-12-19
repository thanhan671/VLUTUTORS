using System;
using System.Collections.Generic;

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
        public string TenQuyen { get; set; }

        public virtual ICollection<Taikhoanadmin> Taikhoanadmins { get; set; }
    }
}

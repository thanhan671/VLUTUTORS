using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace VLUTUTORS.Models
{
    public partial class Baikiemtra
    {
        public int IdCauHoi { get; set; }
        public string CauHoi { get; set; }
        public string DapAnA { get; set; }
        public string DapAnB { get; set; }
        public string DapAnC { get; set; }
        public string DapAnD { get; set; }
        public string DapAnDung { get; set; }

        [NotMapped]
        public string aChecked { get; set; }
        [NotMapped]
        public string bChecked { get; set; }
        [NotMapped]
        public string cChecked { get; set; }
        [NotMapped]
        public string dChecked { get; set; }

        [NotMapped]
        public List<Baikiemtra> quizes { get; set; }
    }
}

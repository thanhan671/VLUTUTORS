﻿using System;

namespace VLUTUTORS.Models
{
    public class Danhgiagiasu
    {
        public int Id { get; set; }
        public int GiasuId { get; set; }
        public int NguoidungId { get; set; }
        public int Diem { get; set; }  

        public string Danhgia { get; set; }

        public DateTime NgayTao { get; set; }
    }
}
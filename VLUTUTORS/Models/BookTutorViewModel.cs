﻿using System.Collections.Generic;

namespace VLUTUTORS.Models
{
    public class BookTutorViewModel
    {
        public Taikhoannguoidung Tutor { get; set; }

        public Mongiasu Subject1 { get; set; }
        public Mongiasu Subject2 { get; set; }

        public bool IsInWishlish { get; set; }
        public List<CommentViewModel> Commnents { get; set; }
    }

    public class CommentViewModel
    {
        public Danhgiagiasu Comment;
        public Taikhoannguoidung NguoiDanhGia;
    }
}
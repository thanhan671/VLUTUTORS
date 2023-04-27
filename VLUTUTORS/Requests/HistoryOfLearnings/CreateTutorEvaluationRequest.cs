﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VLUTUTORS.Requests.HistoryOfLearnings
{
    public class CreateTutorEvaluationRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Không tìm thấy thông tin ca dạy !")]
        public int LessonId { get; set; }
        public int? Rating { get; set; }
        public string FeedBack { get; set; }
        public List<int> Skills { get; set; }
        public bool Wishlish { get; set; }
    }
}

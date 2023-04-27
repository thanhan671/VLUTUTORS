using System;

namespace VLUTUTORS.Responses.HistoryOfLearnings
{
    public class GetAllHistoryOfLearningResponse
    {
        public string TenMonGiaSu { get; set; }

        public DateTime? NgayDay { get; set; }

        public string KhoangThoiGian { get; set; }

        public string GiaTien { get; set; }
        public bool? TrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public string TenGiaSu { get; set; }
        public int? IdGiaSu { get; set; }

        public int IdCaDay { get; set; }
        public bool IsExistTutorEvaluation { get; set; }

        public bool IsExistLearnerEvaluation { get; set; }
    }
}

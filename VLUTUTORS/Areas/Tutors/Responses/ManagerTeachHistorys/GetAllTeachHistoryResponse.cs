using System;

namespace VLUTUTORS.Areas.Tutors.Responses.ManagerTeachHistorys
{
    public class GetAllTeachHistoryResponse
    {
        public int IdNguoiDay { get; set; }

        public string TenMonGiaSu { get; set; }

        public int? IdNguoiHoc { get; set; }
        public string TenNguoiHoc { get; set; }

        public DateTime? NgayDay { get; set; }

        public string ThoiGianBatDau { get; set; }
        public string ThoiGianKetThuc { get; set; }

        public string GiaTien { get; set; }
        public bool? TrangThai { get; set; }
        public string TenTrangThai { get; set; }

        public int IdCaDay { get; set; }

        public bool IsExistTutorEvaluation { get; set; }

        public bool IsExistLearnerEvaluation { get; set; }
    }
}

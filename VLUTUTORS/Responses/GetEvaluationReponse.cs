using System;
using System.Collections.Generic;

namespace VLUTUTORS.Responses
{
    public class GetEvaluationReponse
    {
        public int? IdNguoiDay { get; set; }
        public string TenNguoiDay { get; set; }
        public string TenMonGiaSu { get; set; }

        public int? IdNguoiHoc { get; set; }
        public string TenNguoiHoc { get; set; }

        public int? IdCaDay { get; set; }
        public string DanhGia { get; set; }
        public int? Diem { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayDay { get; set; }
        public IList<CriteriaEvaluationModel> CriteriaEvaluations { get; set; }
    }
    public class CriteriaEvaluationModel
    {
        public int? IdTieuChi { get; set; }
        public string TenTieuChi { get; set; }
    }
}

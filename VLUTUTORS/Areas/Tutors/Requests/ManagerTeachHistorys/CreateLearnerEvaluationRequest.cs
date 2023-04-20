using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VLUTUTORS.Areas.Tutors.Requests.ManagerTeachHistorys
{
    public class CreateLearnerEvaluationRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Không tìm thấy thông tin ca dạy !")]
        public int IdCaDay { get; set; }
        public int? Diem { get; set; }
        public string DanhGia { get; set; }
        public List<int> TieuChi { get; set; }
    }
}

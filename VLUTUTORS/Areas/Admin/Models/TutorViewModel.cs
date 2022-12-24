using Microsoft.AspNetCore.Mvc.Rendering;

namespace VLUTUTORS.Models
{
    public class TutorViewModel
    {
        public Taikhoannguoidung Tutor { get; set; }
        public string Subject1 { get; set; }
        public string Subject2 { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public string Bank { get; set; }
        public string TestingPoint { get; set; }
        public string ApprovedStatus { get; set; }

        public SelectList Status { get; set; }
    }
}

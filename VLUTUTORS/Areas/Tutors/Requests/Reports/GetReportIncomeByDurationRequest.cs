using System;

namespace VLUTUTORS.Areas.Tutors.Requests.Reports
{
    public class GetReportIncomeByDurationRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

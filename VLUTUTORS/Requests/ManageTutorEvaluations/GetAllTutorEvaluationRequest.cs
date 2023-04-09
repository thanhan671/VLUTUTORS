using System;

namespace VLUTUTORS.Requests.ManageTutorEvaluations
{
    public class GetAllTutorEvaluationRequest : PagingRequest
    {
        public string Search { get; set; }
        public int? RateStar { get; set; }
        public int? Criteria { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string StatusCode { get; set; }
    }
}

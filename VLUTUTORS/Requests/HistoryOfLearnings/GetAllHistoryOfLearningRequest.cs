using System;

namespace VLUTUTORS.Requests.HistoryOfLearnings
{
    public class GetAllHistoryOfLearningRequest : PagingRequest
    {
        public string Search { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? StatusCode { get; set; }
    }
}

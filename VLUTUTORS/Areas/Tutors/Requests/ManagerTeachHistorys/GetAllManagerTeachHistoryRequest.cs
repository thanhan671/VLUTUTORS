using System;
using VLUTUTORS.Requests;

namespace VLUTUTORS.Areas.Tutors.Requests.ManagerTeachHistorys
{
    public class GetAllManagerTeachHistoryRequest : PagingRequest
    {
        public string Search { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

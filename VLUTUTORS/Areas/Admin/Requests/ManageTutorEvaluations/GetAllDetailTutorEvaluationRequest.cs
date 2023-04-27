using VLUTUTORS.Requests;

namespace VLUTUTORS.Areas.Admin.Requests.ManageTutorEvaluations
{
    public class GetAllDetailTutorEvaluationRequest : PagingRequest
    {
        public string Search { get; set; }
    }
}

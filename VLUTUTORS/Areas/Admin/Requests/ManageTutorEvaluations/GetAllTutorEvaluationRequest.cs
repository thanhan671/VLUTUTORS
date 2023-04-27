using VLUTUTORS.Requests;

namespace VLUTUTORS.Areas.Admin.Requests.ManageTutorEvaluations
{
    public class GetAllTutorEvaluationRequest : PagingRequest
    {
        public string Search { get; set; }
    }
}

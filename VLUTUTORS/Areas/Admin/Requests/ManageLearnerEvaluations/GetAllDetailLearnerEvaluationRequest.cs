using VLUTUTORS.Requests;

namespace VLUTUTORS.Areas.Admin.Requests.ManageLearnerEvaluations
{
    public class GetAllDetailLearnerEvaluationRequest : PagingRequest
    {
        public string Search { get; set; }
    }
}

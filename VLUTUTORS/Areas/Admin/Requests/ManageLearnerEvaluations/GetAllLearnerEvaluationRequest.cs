using VLUTUTORS.Requests;

namespace VLUTUTORS.Areas.Admin.Requests.ManageLearnerEvaluations
{
    public class GetAllLearnerEvaluationRequest : PagingRequest
    {
        public string Search { get; set; }
    }
}

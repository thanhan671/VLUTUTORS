using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VLUTUTORS.Common;

namespace VLUTUTORS.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}

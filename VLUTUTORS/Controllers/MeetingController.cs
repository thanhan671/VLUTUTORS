using Microsoft.AspNetCore.Mvc;

namespace VLUTUTORS.Controllers
{
    public class MeetingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

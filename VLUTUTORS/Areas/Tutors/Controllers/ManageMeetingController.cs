using Microsoft.AspNetCore.Mvc;

namespace VLUTUTORS.Areas.Tutors.Controllers

{
    [Area("Tutors")]
    public class ManageMeetingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ManageHistoryBooking()
        {
            return View();
        }
    }
}

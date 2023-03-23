using Microsoft.AspNetCore.Mvc;

namespace VLUTUTORS.Areas.Tutors.Controllers

{
    [Area("Tutors")]
    public class ManageTeachHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

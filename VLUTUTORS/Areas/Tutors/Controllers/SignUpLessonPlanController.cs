using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VLUTUTORS.Areas.Tutors.Controllers
{
    [Area("Tutors")]
    public class SignUpLessonPlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EditLessonPlan()
        {
            return View();
        }
    }
}
